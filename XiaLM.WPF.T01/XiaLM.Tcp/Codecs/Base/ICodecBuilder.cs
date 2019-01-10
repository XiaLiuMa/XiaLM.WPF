using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Codecs
{
    /// <summary>
    /// 解码器接口
    /// </summary>
    public interface ICodecBuilder
    {
        /// <summary>
        /// 解码
        /// </summary>
        ICodecDecoding DecodingBuilder { get; }
        /// <summary>
        /// 编码
        /// </summary>
        ICodecEncoding EncodingBuilder { get; }
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        ICodecBuilder Clone();
    }
}
