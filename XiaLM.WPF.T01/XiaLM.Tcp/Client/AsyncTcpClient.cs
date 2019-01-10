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
using XiaLM.Tcp.Codecs;
using XiaLM.Tcp.Extension;
using XiaLM.Tcp.Mod;

namespace XiaLM.Tcp.Client
{
    public class AsyncTcpClient
    {
        private IPEndPoint _endPoint;
        private IAsyncTcpClientMessage _registration;
        private AsyncTcpClientConfiguration _configuration;
        private Stream _stream;
        private TcpClient _tcpClient;
        private int _state;
        private const int _none = 0;
        private const int _connecting = 1;
        private const int _connected = 2;
        private const int _closed = 5;
        private int _bufferPool = 1024 * 1024 * 1;//默认10M

        private readonly BufferManage bufferManage = null;

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
                    case _closed:
                        return TcpConnectionStatus.Closed;
                    default:
                        return TcpConnectionStatus.Closed;
                }
            }
        }

        public TimeSpan ConnectTimeout { get { return _configuration.ConnectTimeout; } }
        private bool Connected { get { return _tcpClient != null && _tcpClient.Client.Connected; } }

        public IPEndPoint RemoteEndPoint => Connected ? (IPEndPoint)_tcpClient.Client.RemoteEndPoint : _endPoint;

        public IPEndPoint LocalEndPoint => Connected ? (IPEndPoint)_tcpClient.Client.LocalEndPoint : _endPoint;


        public event ServerConnected OnServerConnected;
        public event ServerDataReceived OnServerDataReceived;
        public event ServerDisconnected OnServerDisconnected;
        public event ServerError OnServerError;

        /// <summary>
        /// tcp客户端
        /// </summary>
        /// <param name="endPoint">服务端地址</param>
        /// <param name="registration">消息注册体</param>

        public AsyncTcpClient(IPEndPoint endPoint, IAsyncTcpClientMessage registration) : this(endPoint, registration, new NoResolutionCodecBuilder(), null, 1024 * 1024 * 1)
        {
        }
        /// <summary>
        /// tcp客户端
        /// </summary>
        /// <param name="endPoint">服务端地址</param>
        /// <param name="registration">消息注册体</param>
        /// <param name="builder">解码器</param>
        public AsyncTcpClient(IPEndPoint endPoint, IAsyncTcpClientMessage registration, ICodecBuilder builder) : this(endPoint, registration, builder, null, 1024 * 1024 * 1)
        {

        }
        /// <summary>
        /// tcp客户端
        /// </summary>
        /// <param name="endPoint">服务端地址</param>
        /// <param name="registration">消息注册体</param>
        /// <param name="builder">解码器</param>
        /// <param name="configuration">配置</param>
        public AsyncTcpClient(IPEndPoint endPoint, IAsyncTcpClientMessage registration, ICodecBuilder builder, AsyncTcpClientConfiguration configuration) : this(endPoint, registration, builder, configuration, 1024 * 1024 * 1)
        {

        }
        /// <summary>
        /// tcp客户端
        /// </summary>
        /// <param name="endPoint">服务端地址</param>
        /// <param name="registration">消息注册体</param>
        /// <param name="builder">解码器</param>
        /// <param name="bufferPool">缓冲池大小</param>
        /// <param name="isRestConn">是否重连</param>
        public AsyncTcpClient(IPEndPoint endPoint, IAsyncTcpClientMessage registration, ICodecBuilder builder, int bufferPool) : this(endPoint, registration, builder, null, bufferPool)
        {

        }

        /// <summary>
        /// tcp客户端
        /// </summary>
        /// <param name="endPoint">服务端地址</param>
        /// <param name="registration">消息注册体</param>
        /// <param name="builder">解码器</param>
        /// <param name="configuration">配置</param>
        /// <param name="bufferPool">缓冲池大小</param>
        /// <param name="isRestConn">是否重连</param>
        public AsyncTcpClient(IPEndPoint endPoint, IAsyncTcpClientMessage registration, ICodecBuilder builder, AsyncTcpClientConfiguration configuration, int bufferPool)
        {
            this._endPoint = endPoint;
            this._registration = registration;
            this._configuration = configuration == null ? new AsyncTcpClientConfiguration() : configuration;
            this._bufferPool = bufferPool;
            bufferManage = new BufferManage(builder);
            bufferManage.ReceiveDataEvent += BufferManage_ReceiveDataEvent;
        }
        private  void BufferManage_ReceiveDataEvent(TcpBuffer obj)
        {
            if (this.State != TcpConnectionStatus.Connected) return;
            if (this.OnServerDataReceived != null)
                 this.OnServerDataReceived?.Invoke(this, obj.Datas, obj.Offset, obj.Count).Wait();
            if (_registration != null)
                 this._registration?.OnServerDataReceived(this, obj.Datas, obj.Offset, obj.Count).Wait();
        }

        public async Task Close()
        {
            await Close(true);
        }
        private async Task Close(bool shallNotifyUserSide)
        {
            if (Interlocked.Exchange(ref _state, _closed) == _closed)
            {
                return;
            }
            Shutdown();
            if (shallNotifyUserSide)
            {
                try
                {
                    if (this.OnServerDisconnected != null)
                        await this.OnServerDisconnected?.Invoke(this);
                    if (_registration != null)
                        await _registration?.OnServerDisconnected(this);
                }
                catch (Exception ex) // catch all exceptions from out-side
                {
                    if (this.OnServerError != null)
                        await this.OnServerError?.Invoke(ex);
                    if (_registration != null)
                        await _registration?.OnServerError(ex);
                }
            }
            Clean();
        }
        public async Task Connect()
        {
            int origin = Interlocked.Exchange(ref _state, _connecting);
            if (!(origin == _none || origin == _closed))
            {
                await Close(false); // connecting with wrong state
                throw new InvalidOperationException("This tcp socket client is in invalid state when connecting.");
            }
            Clean();
            try
            {
                _tcpClient = new TcpClient(_endPoint.Address.AddressFamily);
                SetSocketOptions();
                var awaiter = _tcpClient.ConnectAsync(_endPoint.Address, _endPoint.Port);
                if (!awaiter.Wait(ConnectTimeout))
                {
                    await Close(false);
                    throw new TimeoutException(string.Format("连接超时[{0}:{1}]", _endPoint.Address, _endPoint.Port));
                }
                _stream = _tcpClient.GetStream();
                if (Interlocked.CompareExchange(ref _state, _connected, _connecting) != _connecting)
                {
                    await Close(false); // connected with wrong state
                    throw new InvalidOperationException("This tcp socket client is in invalid state when connected.");
                }
                bool iserror = false;
                try
                {
                    if (this.OnServerConnected != null)
                        await this.OnServerConnected?.Invoke(this);
                    if (_registration != null)
                        await _registration?.OnServerConnected(this);
                }
                catch (Exception ex)
                {
                    iserror = true;
                    if (this.OnServerError != null)
                        await this.OnServerError?.Invoke(ex);
                    if (_registration != null)
                        await _registration?.OnServerError(ex);
                }
                if (!iserror)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        try
                        {
                            byte[] bs = new byte[this._bufferPool];
                            while (State == TcpConnectionStatus.Connected)
                            {
                                int count = await _stream.ReadAsync(bs, 0, bs.Length);
                                if (count == 0) break;
                                bufferManage.Receive(bs.Take(count).ToArray());
                            }
                            throw new Exception("连接已经断开");
                        }
                        catch (Exception ex)
                        {
                            if (this.OnServerError != null)
                                await this.OnServerError?.Invoke(ex);
                            if (_registration != null)
                                await _registration?.OnServerError(ex);
                            await Close(true);
                        }

                    }).Employ();
                }
                else
                {
                    await Close(true);
                }
            }
            catch (Exception ex)
            {
                await Close(true);
                if (this.OnServerError != null)
                    await this.OnServerError?.Invoke(ex);
                if (_registration != null)
                    await _registration?.OnServerError(ex);
            }
        }
        private void SetSocketOptions()
        {
            _tcpClient.ReceiveBufferSize = _configuration.ReceiveBufferSize;
            _tcpClient.SendBufferSize = _configuration.SendBufferSize;
            _tcpClient.ReceiveTimeout = (int)_configuration.ReceiveTimeout.TotalMilliseconds;
            _tcpClient.SendTimeout = (int)_configuration.SendTimeout.TotalMilliseconds;
            _tcpClient.NoDelay = _configuration.NoDelay;
            _tcpClient.LingerState = _configuration.LingerState;

            if (_configuration.KeepAlive)
            {
                _tcpClient.Client.SetSocketOption(
                    SocketOptionLevel.Socket,
                    SocketOptionName.KeepAlive,
                    (int)_configuration.KeepAliveInterval.TotalMilliseconds);
            }

            _tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _configuration.ReuseAddress);
        }
        public async Task SendAsync(byte[] data)
        {
            if (this.State != TcpConnectionStatus.Connected) return;
            if (data == null || data.Length <= 0) return;
            var buffer = bufferManage.GetSendBuffer(data);
            await _stream.WriteAsync(buffer.Datas, buffer.Offset, buffer.Count);
        }
        private void Clean()
        {
            try
            {
                try
                {
                    if (_stream != null)
                    {
                        _stream.Dispose();
                    }
                }
                catch { }
                try
                {
                    if (_tcpClient != null)
                    {
                        _tcpClient.Close();
                    }
                }
                catch { }
            }
            catch { }
            finally
            {
                _stream = null;
                _tcpClient = null;
                //bufferManage.Dispose();
                //GC.SuppressFinalize(this);//不能在这里GC，其他地方如果引用该对象可能造成GC访问报错，程序崩溃！
            }
        }
        public void Shutdown()
        {
            if (_tcpClient != null && _tcpClient.Connected)
            {
                _tcpClient.Client.Shutdown(SocketShutdown.Send);
            }
        }
    }
}
