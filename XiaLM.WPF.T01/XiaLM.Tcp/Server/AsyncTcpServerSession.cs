using XiaLM.Tcp.Codecs;
using XiaLM.Tcp.Extension;
using XiaLM.Tcp.Mod;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Server
{
    public class AsyncTcpServerSession
    {
        private IPEndPoint _remoteEndPoint;
        private IPEndPoint _localEndPoint;
        private TcpClient _client;
        private AsyncTcpServerConfiguration _configuration;
        private AsyncTcpServer _server;
        private IAsyncTcpServerSessionMessage _registration;
        private readonly BufferManage bufferManage = null;
        private Stream stream;
        private int _bufferPool = 1024 * 1024 * 10;//默认10M

        private int _state;
        private const int _none = 0;
        private const int _connecting = 1;
        private const int _connected = 2;
        private const int _disposed = 5;
        public TcpConnectionStatus State
        {
            get
            {
                switch (_state)
                {
                    case _none:
                        return TcpConnectionStatus.None;
                    case _connecting:
                        return TcpConnectionStatus.Connecting;
                    case _connected:
                        return TcpConnectionStatus.Connected;
                    case _disposed:
                        return TcpConnectionStatus.Closed;
                    default:
                        return TcpConnectionStatus.Closed;
                }
            }
        }
        public string SessionKey { get; private set; }
        public DateTime StartTime { get; private set; }
        private bool Connected { get { return _client != null && _client.Client.Connected; } }
        public IPEndPoint RemoteEndPoint { get { return Connected ? (IPEndPoint)_client.Client.RemoteEndPoint : _remoteEndPoint; } }
        public IPEndPoint LocalEndPoint { get { return Connected ? (IPEndPoint)_client.Client.LocalEndPoint : _localEndPoint; } }

        public Socket Socket { get { return Connected ? _client.Client : null; } }
        public Stream Stream { get { return stream; } }
        public AsyncTcpServer Server { get { return _server; } }

        public AsyncTcpServerSession(TcpClient client, AsyncTcpServerConfiguration configuration, AsyncTcpServer server, IAsyncTcpServerSessionMessage registration, ICodecBuilder builder, int bufferPool)
        {
            SessionKey = Guid.NewGuid().ToString();
            this._client = client;
            this._configuration = configuration;
            this._server = server;
            this._registration = registration;
            SetSocketOptions();
            _remoteEndPoint = this.RemoteEndPoint;
            _localEndPoint = this.LocalEndPoint;
            bufferManage = new BufferManage(builder);
            bufferManage.ReceiveDataEvent += BufferManage_ReceiveDataEvent;
            this._bufferPool = bufferPool;
        }

        private  void BufferManage_ReceiveDataEvent(TcpBuffer obj)
        {
            if (this.State != TcpConnectionStatus.Connected)
            {
                return;
            }
             this._server?.SessionDataReceived(this, obj.Datas, obj.Offset, obj.Count).Wait();
            if (_registration != null)
                 _registration?.OnSessionDataReceived(this, obj.Datas, obj.Offset, obj.Count).Wait();
        }

        public async Task Start()
        {
            var intloke = Interlocked.CompareExchange(ref _state, _connecting, _none);
            if (intloke.Equals(_disposed))
            {
                throw new ObjectDisposedException("当前连接已经关闭");
            }
            else if (intloke != _none)
            {
                throw new InvalidOperationException("当前已经开始连接");
            }
            try
            {
                stream = _client.GetStream();
                intloke = Interlocked.CompareExchange(ref _state, _connected, _connecting);
                if (!intloke.Equals(_connecting))
                {
                    await Close(false);
                    throw new ObjectDisposedException("当前连接已经释放");
                }
                bool iserror = false;
                try
                {
                    await this._server?.SessionStarted(this);
                    if (_registration != null)
                        await _registration?.OnSessionStarted(this);
                }
                catch (Exception ex)
                {
                    iserror = true;
                }
                if (!iserror)
                {

                    try
                    {
                        byte[] bs = new byte[this._bufferPool];
                        while (State == TcpConnectionStatus.Connected)
                        {
                            int count = await stream.ReadAsync(bs, 0, bs.Length);
                            if (count == 0) break;
                            bufferManage.Receive(bs.Take(count).ToArray());
                        }
                        throw new Exception("连接已经断开");
                    }
                    catch (Exception ex)
                    {
                        await this._server?.SessionError(ex);
                        if (_registration != null)
                            await _registration?.OnSessionError(ex);
                        await Close(true);
                    }
                }
                else
                {
                    await Close(true);
                }

            }
            catch (Exception ex)
            {
                await Close(true);
                throw;
            }
        }
        public async Task Close()
        {
            await Close(true);
        }
        private async Task Close(bool shallNotifyUserSide)
        {
            if (Interlocked.Exchange(ref _state, _disposed) == _disposed)
            {
                return;
            }
            Shutdown();
            if (shallNotifyUserSide)
            {
                await this._server?.SessionClosed(this);
                if (_registration != null)
                    await _registration?.OnSessionClosed(this);
            }
            Dispose();
        }
        public void Shutdown()
        {
            if (_client != null && _client.Connected)
            {
                _client.Client.Shutdown(SocketShutdown.Send);
            }
        }
        private async void Dispose()
        {
            try
            {
                try
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    await this._server?.SessionError(ex);
                    if (_registration != null)
                        await _registration?.OnSessionError(ex);
                }
                try
                {
                    if (_client != null)
                    {
                        _client.Close();
                    }
                }
                catch (Exception ex)
                {
                    await this._server?.SessionError(ex);
                    if (_registration != null)
                        await _registration?.OnSessionError(ex);
                }
            }
            catch (Exception ex)
            {
                await this._server?.SessionError(ex);
                if (_registration != null)
                    await _registration?.OnSessionError(ex);
            }
            finally
            {
                stream = null;
                _client = null;
                //bufferManage.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        private void SetSocketOptions()
        {
            _client.ReceiveBufferSize = _configuration.ReceiveBufferSize;
            _client.SendBufferSize = _configuration.SendBufferSize;
            _client.ReceiveTimeout = (int)_configuration.ReceiveTimeout.TotalMilliseconds;
            _client.SendTimeout = (int)_configuration.SendTimeout.TotalMilliseconds;
            _client.NoDelay = _configuration.NoDelay;
            _client.LingerState = _configuration.LingerState;

            if (_configuration.KeepAlive)
            {
                _client.Client.SetSocketOption(
                    SocketOptionLevel.Socket,
                    SocketOptionName.KeepAlive,
                    (int)_configuration.KeepAliveInterval.TotalMilliseconds);
            }

            _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _configuration.ReuseAddress);
        }

        public async Task SendAsync(byte[] data)
        {
            try
            {
                if (this.State != TcpConnectionStatus.Connected) return;
                if (data == null || data.Length <= 0) return;
                var buffer = bufferManage.GetSendBuffer(data);
                await stream.WriteAsync(buffer.Datas, buffer.Offset, buffer.Count);
            }
            catch (Exception ex)
            {
                await this._server?.SessionError(ex);
                if (_registration != null)
                    await _registration?.OnSessionError(ex);
            }
        }
    }
}
