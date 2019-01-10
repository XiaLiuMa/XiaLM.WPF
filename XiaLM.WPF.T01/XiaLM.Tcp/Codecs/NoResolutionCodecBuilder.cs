using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Codecs
{
    public class NoResolutionCodecBuilder : ICodecBuilder
    {
        public ICodecDecoding DecodingBuilder => new NoResolutionCodecDecoding();

        public ICodecEncoding EncodingBuilder => new NoResolutionCodecEncoding();
        public ICodecBuilder Clone()
        {
            return new NoResolutionCodecBuilder();
        }
    }
    public class NoResolutionCodecDecoding : ICodecDecoding
    {
        public void Decoding(TcpBuffer playload, DataAnalysisResults callback)
        {
            callback(playload);
        }
    }
    public class NoResolutionCodecEncoding : ICodecEncoding
    {
        public TcpBuffer Encoding(TcpBuffer buffer)
        {
            return buffer;
        }
    }
}
