using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp
{
    public class TcpBuffer
    {
        public TcpBuffer() { }
        public TcpBuffer(byte[] datas, int offset, int count)
        {
            this.Datas = datas;
            this.Offset = offset;
            this.Count = count;
        }
        public byte[] Datas { get; set; }
        public int Offset { get; set; }

        public int Count { get; set; }
    }
}
