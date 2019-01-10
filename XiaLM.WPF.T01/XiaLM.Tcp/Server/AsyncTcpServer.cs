using XiaLM.Tcp.Codecs;
using XiaLM.Tcp.Extension;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Server
{
    public class AsyncTcpServer
    {
        private TcpListener listener;
        private int _state;
        private const int _none = 0;
        private const int _listening = 1;
        private const int _disposed = 2;
        private IPEndPoint _endPoint;
        private AsyncTcpServerConfiguration _configuration;
        private readonly ConcurrentDictionary<string, AsyncTcpServerSession> _sessions = new ConcurrentDictionary<string, AsyncTcpServerSession>();
        private ICodecBuilder _builder;
        private IAsyncTcpServerSessionMessage _registration;
        public event Action<Exception> ErrorEvent = (e) => { };
        private int _bufferPool = 1024 * 1024 * 1;//默认1M
        public IPEndPoint ListenedEndPoint { get; private set; }
        public int SessionCount { get { return _sessions.Count; } }
        public bool Islistening { get { return _state == _listening; } }

        public event SessionStarted OnSessionStarted;
        public event SessionDataReceived OnSessionDataReceived;
        public event SessionClosed OnSessionClosed;
        public event SessionError OnSessionError;

        public AsyncTcpServer(IPEndPoint endPoint, IAsyncTcpServerSessionMessage registration) : this(endPoint, registration, new NoResolutionCodecBuilder(), null, 1024 * 1024 * 1)
        {
        }
        public AsyncTcpServer(IPEndPoint endPoint, IAsyncTcpServerSessionMessage registration, ICodecBuilder builder) : this(endPoint, registration, builder, null, 1024 * 1024 * 1)
        {
        }
        public AsyncTcpServer(IPEndPoint endPoint, IAsyncTcpServerSessionMessage registration, ICodecBuilder builder, AsyncTcpServerConfiguration configuration, int bufferPool)
        {
            _endPoint = endPoint;
            _configuration = configuration == null ? new AsyncTcpServerConfiguration() : configuration;
            _builder = builder;
            _registration = registration;
            ListenedEndPoint = _endPoint;
            this._bufferPool = bufferPool;
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        public void Listener()
        {
            try
            {
                int origin = Interlocked.CompareExchange(ref _state, _listening, _none);
                if (origin.Equals(_disposed))
                {
                    throw new ObjectDisposedException("连接已断开");
                }
                else if (origin != _none)
                {
                    throw new InvalidOperationException("侦听已开始");
                }
                try
                {
                    listener = new TcpListener(_endPoint);
                    SetSocketOptions();
                    listener.Start(_configuration.BacklogConnections);
                    Task.Factory.StartNew(async () =>
                    {
                        await Accept();
                    });
                }
                catch (Exception ex)
                {
                    ErrorEvent(ex);
                }
            }
            catch (Exception ex)
            {
                ErrorEvent(ex);
            }

        }

        public bool Pending()
        {
            if (!Islistening)
            {
                throw new InvalidOperationException("Tcp服务未激活");
            }
            return listener.Pending();
        }

        public void Shutdown()
        {
            if (Interlocked.Exchange(ref _state, _disposed) == _disposed)
            {
                return;
            }

            try
            {
                listener.Stop();
                listener = null;

                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        foreach (var session in _sessions.Values)
                        {
                            await session.Close(); // parent server close session when shutdown
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorEvent(ex);
                    }
                },
                TaskCreationOptions.PreferFairness)
                .Wait();
            }
            catch (Exception ex)
            {
                ErrorEvent(ex);
            }
        }
        private async Task Accept()
        {
            try
            {
                while (Islistening)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Task.Factory.StartNew(async () =>
                    {
                        await ClientInfo(client);
                    }).Employ();
                }
            }
            catch (Exception ex)
            {
                ErrorEvent(ex);
            }
        }
        private async Task ClientInfo(TcpClient client)
        {
            var session = new AsyncTcpServerSession(client, _configuration, this, _registration, _builder.Clone(), this._bufferPool);
            if (_sessions.TryAdd(session.SessionKey, session))
            {
                try
                {
                    await session.Start();
                }
                catch (Exception ex)
                {
                    ErrorEvent(ex);
                }
                finally
                {
                    AsyncTcpServerSession throwAway;
                    if (_sessions.TryRemove(session.SessionKey, out throwAway))
                    {
                        ErrorEvent(new Exception("客户端已断开连接"));
                    }
                }
            }
        }
        private void SetSocketOptions()
        {
            listener.AllowNatTraversal(_configuration.AllowNatTraversal);
            listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _configuration.ReuseAddress);
        }
        public async Task BroadcastAsync(byte[] data)
        {
            foreach (var session in _sessions.Values)
            {
                await session.SendAsync(data);
            }
        }
        public async Task SendAsync(string sessionKey, byte[] data)
        {
            var obj = _sessions.Where(r => r.Value.SessionKey.Equals(sessionKey)).FirstOrDefault();
            await obj.Value.SendAsync(data);
        }
        internal async Task SessionStarted(AsyncTcpServerSession session)
        {
            if (this.OnSessionStarted != null)
                await this.OnSessionStarted?.Invoke(session);
        }

        internal async Task SessionDataReceived(AsyncTcpServerSession session, byte[] data, int offset, int count)
        {
            if (this.OnSessionDataReceived != null)
                await this.OnSessionDataReceived?.Invoke(session, data, offset, count);
        }

        internal async Task SessionClosed(AsyncTcpServerSession session)
        {
            if (this.OnSessionClosed != null)
                await this.OnSessionClosed?.Invoke(session);
        }

        internal async Task SessionError(Exception ex)
        {
            if (this.OnSessionError != null)
                await this.OnSessionError?.Invoke(ex);
        }
    }
}
