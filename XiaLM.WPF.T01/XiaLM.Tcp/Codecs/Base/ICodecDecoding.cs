using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Codecs
{
    public delegate void DataAnalysisResults(TcpBuffer buffer);
    public interface ICodecDecoding
    {
        void Decoding(TcpBuffer playload, DataAnalysisResults callback);
    }
}
