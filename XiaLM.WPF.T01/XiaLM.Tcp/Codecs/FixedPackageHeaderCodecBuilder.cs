using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Codecs
{
    //            数据头标识      数据长度(不加校验码长度)           数据区            校验码
    //数据结构为 [0xEA,0x56]   +        [lenght]                  +   [data]    +     [crc16-ccitt].
    public class FixedPackageHeaderCodecBuilder : ICodecBuilder
    {
        private readonly bool _isCrc = false;
        private ICodecDecoding decoding;
        private ICodecEncoding encoding;
        public FixedPackageHeaderCodecBuilder()
        {
            decoding = new FixedPackageHeaderDecodingBuilder(_isCrc);
            encoding = new FixedPackageHeaderEncodingBuilder(_isCrc);

        }
        public FixedPackageHeaderCodecBuilder(bool isCRC)
        {
            _isCrc = isCRC;
            decoding = new FixedPackageHeaderDecodingBuilder(_isCrc);
            encoding = new FixedPackageHeaderEncodingBuilder(_isCrc);
        }
        public ICodecDecoding DecodingBuilder => decoding;

        public ICodecEncoding EncodingBuilder => encoding;

        public ICodecBuilder Clone()
        {
            return new FixedPackageHeaderCodecBuilder(_isCrc);
        }
    }
    public class FixedPackageHeaderDecodingBuilder : ICodecDecoding
    {
        List<byte> _list = new List<byte>();
        List<byte> datas = new List<byte>();
        private int dataLenght = 0;
        private byte[] _head = new byte[2] { 0xEA, 0x56 };
        private readonly bool _isCrc = false;
        private CRC crc = new CRC();
        private readonly int fixHeadLenght;
        public FixedPackageHeaderDecodingBuilder(bool isCRC)
        {
            _isCrc = isCRC;
            fixHeadLenght = 6;
        }
        public void Decoding(TcpBuffer playload, DataAnalysisResults callback)
        {
            _list.AddRange(playload.Datas);
            while (true)
            {
                if (datas.Count == 0)
                {
                    if (_list.Count >= 2)
                    {
                        byte[] head = _list.Take(2).ToArray();
                        if (isHead(head))
                        {
                            if (_list.Count <= fixHeadLenght) return;
                            byte crc = _list.Skip(2).FirstOrDefault();
                            int length = BitConverter.ToInt32(_list.Skip(2).Take(4).Reverse().ToArray(), 0);
                            if (length < 0)
                            {
                                _list.RemoveAt(0);
                                continue;
                            }
                            dataLenght = length + (_isCrc ? 2 : 0);//如果crc则数据长度加2
                            int tempLength = (_list.Count - fixHeadLenght);
                            if ((tempLength - dataLenght) == 0)
                            {
                                datas.AddRange(_list.Skip(fixHeadLenght));
                                _list.RemoveRange(0, fixHeadLenght + dataLenght);
                                dataResult(datas, callback);
                                break;
                            }
                            else
                            if ((tempLength - dataLenght) < 0)
                            {
                                datas.AddRange(_list.Skip(fixHeadLenght));
                            }
                            else
                            {
                                datas.AddRange(_list.Skip(fixHeadLenght).Take(dataLenght));
                            }
                            _list.RemoveRange(0, fixHeadLenght + datas.Count);
                        }
                        else
                        {
                            _list.RemoveAt(0);
                        }
                    }
                    else
                    {
                        if (_list.Count <= 0) break;
                        if (_list.Count == 1)
                        {
                            if (!_list.First().Equals(0xEA))
                            {
                                _list.RemoveAt(0);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    int d = datas.Count;
                    int c = dataLenght - d;
                    if (c == 0)
                    {
                        dataResult(datas, callback);
                        continue;
                    }
                    else
                    {
                        if (_list.Count == 0) break;
                        if (_list.Count >= c)
                        {
                            datas.AddRange(_list.Take(c));
                            _list.RemoveRange(0, c);
                        }
                        else
                        {
                            datas.AddRange(_list);
                            _list.RemoveRange(0, _list.Count);
                        }
                    }

                }


            }
        }
        private void dataResult(List<byte> list, DataAnalysisResults callback)
        {
            if (_isCrc)
            {
                byte[] data = list.Take(list.Count - 2).ToArray();
                ushort crcNum = BitConverter.ToUInt16(list.Skip(list.Count - 2).Reverse().ToArray(), 0);//数据的校验码
                var cn = crc.CRC16_CCITT(data);
                if (cn == crcNum)
                {
                    callback?.Invoke(new TcpBuffer(data, 0, data.Length));
                }
            }
            else
            {
                var data = list.ToArray();
                callback?.Invoke(new TcpBuffer(data, 0, data.Length));
            }
            datas.Clear();
            dataLenght = 0;
        }
        private bool isHead(byte[] bs)
        {
            if (bs.Length != 2) return false;
            var b = true;
            int i = 0;
            foreach (var item in _head)
            {
                if (!item.Equals(bs[i]))
                    b = false;
                i++;
            }
            return b;
        }
    }
    public class FixedPackageHeaderEncodingBuilder : ICodecEncoding
    {
        private byte[] head = new byte[2] { 0xEA, 0x56 };
        private readonly bool _isCrc = false;
        private CRC crc = new CRC();
        public FixedPackageHeaderEncodingBuilder(bool isCRC)
        {
            _isCrc = isCRC;
        }
        public TcpBuffer Encoding(TcpBuffer buffer)
        {
            List<byte> list = new List<byte>();
            int totalLength = _isCrc ? buffer.Count + 2 : buffer.Count;
            list.AddRange(head);//标识头
            list.AddRange(BitConverter.GetBytes(buffer.Count).Reverse().ToArray());//数据区长度
            list.AddRange(buffer.Datas);//数据
            if (_isCrc)
            {
                ushort crcNum = crc.CRC16_CCITT(buffer.Datas);
                byte[] crcBs = BitConverter.GetBytes(crcNum).Reverse().ToArray();//反转改为大端模式
                list.AddRange(crcBs);//校验码
            }
            return new TcpBuffer(list.ToArray(), 0, list.Count);
        }
    }
}
