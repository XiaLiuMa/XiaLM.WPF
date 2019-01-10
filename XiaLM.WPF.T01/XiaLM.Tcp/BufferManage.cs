using XiaLM.Tcp.Codecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp
{
    /// <summary>
    /// 字节管理
    /// </summary>
    public class BufferManage : IDisposable
    {
        private ICodecBuilder _builder = null;
        public event Action<TcpBuffer> ReceiveDataEvent = (s) => { };
        public BufferManage()
        {
        }
        public BufferManage(ICodecBuilder builder) : this()
        {
            _builder = builder;
        }

        public void Receive(byte[] buffer)
        {
                if (buffer == null && buffer.Length <= 0) return;
                var playload = new TcpBuffer() { Datas = buffer };
                this._builder.DecodingBuilder.Decoding(playload, new DataAnalysisResults(dataAnalysis));
        }
        public TcpBuffer GetSendBuffer(byte[] buffer)
        {
            return this._builder.EncodingBuilder.Encoding(new TcpBuffer(buffer, 0, buffer.Length));
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public void dataAnalysis(TcpBuffer buffer)
        {
            this.ReceiveDataEvent?.Invoke(buffer);
        }
    }
}
