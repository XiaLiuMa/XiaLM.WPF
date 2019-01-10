using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Codecs
{
    public interface ICodecEncoding
    {
        TcpBuffer Encoding(TcpBuffer buffer);
    }
}
