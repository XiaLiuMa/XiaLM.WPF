﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Client
{
  public  class AsyncTcpClientConfiguration
    {
        public AsyncTcpClientConfiguration() {
            ReceiveBufferSize = 8192;                   // Specifies the total per-socket buffer space reserved for receives. This is unrelated to the maximum message size or the size of a TCP window.
            SendBufferSize = 8192;                      // Specifies the total per-socket buffer space reserved for sends. This is unrelated to the maximum message size or the size of a TCP window.
            ReceiveTimeout = TimeSpan.Zero;             // Receive a time-out. This option applies only to synchronous methods; it has no effect on asynchronous methods such as the BeginSend method.
            SendTimeout = TimeSpan.Zero;                // Send a time-out. This option applies only to synchronous methods; it has no effect on asynchronous methods such as the BeginSend method.
            NoDelay = true;                             // Disables the Nagle algorithm for send coalescing.
            LingerState = new LingerOption(false, 0);   // The socket will linger for x seconds after Socket.Close is called.
            KeepAlive = false;                          // Use keep-alives.
            KeepAliveInterval = TimeSpan.FromSeconds(5);// https://msdn.microsoft.com/en-us/library/system.net.sockets.socketoptionname(v=vs.110).aspx
            ReuseAddress = false;                       // Allows the socket to be bound to an address that is already in use.
            ConnectTimeout = TimeSpan.FromSeconds(15);
        }
        public int ReceiveBufferSize { get; set; }
        public int SendBufferSize { get; set; }
        public TimeSpan ReceiveTimeout { get; set; }
        public TimeSpan SendTimeout { get; set; }
        public bool NoDelay { get; set; }
        public LingerOption LingerState { get; set; }
        public bool KeepAlive { get; set; }
        public TimeSpan KeepAliveInterval { get; set; }
        public bool ReuseAddress { get; set; }
        public TimeSpan ConnectTimeout { get; set; }
    }
}
