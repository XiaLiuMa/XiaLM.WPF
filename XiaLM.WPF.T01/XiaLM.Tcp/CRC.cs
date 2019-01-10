using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp
{
    public class CRC
    {
        public ushort CRC16_CCITT(byte[] buffer)
        {
            ushort crc = 0x0000;
            foreach (var b in buffer)
            {
                crc = (ushort)(crc ^ (b));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0x8408) : (ushort)(crc >> 1);
                }
            }
            return (ushort)(crc ^ 0x0000);
        }
        public ushort CRC16_CCITT_FALSE(byte[] buffer)
        {
            int crc = 0xFFFF;
            for (int j = 0; j < buffer.Length; j++)
            {
                crc = ((crc >> 8) | (crc << 8)) & 0xffff;
                crc ^= (buffer[j] & 0xff);// byte to int, trunc sign
                crc ^= ((crc & 0xff) >> 4);
                crc ^= (crc << 12) & 0xffff;
                crc ^= ((crc & 0xFF) << 5) & 0xffff;
            }
            crc &= 0xffff;
            return (ushort)crc;
        }
        public ushort CRC16_XMODEM(byte[] buffer)
        {
            int crc = 0x0000; // initial value
            int polynomial = 0x1021; // poly value
            for (int index = 0; index < buffer.Length; index++)
            {
                byte b = buffer[index];
                for (int i = 0; i < 8; i++)
                {
                    bool bit = ((b >> (7 - i) & 1) == 1);
                    bool c15 = ((crc >> 15 & 1) == 1);
                    crc <<= 1;
                    if (c15 ^ bit)
                        crc ^= polynomial;
                }
            }
            crc &= 0xffff;
            return (ushort)crc;

        }
        public ushort CRC16_MODBUS(byte[] buffer)
        {
            ushort crc = 0xFFFF;
            foreach (var b in buffer)
            {
                crc = (ushort)(crc ^ (b));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                }
            }
            return (ushort)(crc ^ 0x0000);
        }
        public ushort CRC16_IBM(byte[] buffer)
        {
            ushort crc = 0x0000;
            foreach (var b in buffer)
            {
                crc = (ushort)(crc ^ (b));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                }
            }
            return (ushort)(crc ^ 0x0000);
        }
        public ushort CRC16_MAXIM(byte[] buffer)
        {
            ushort crc = 0x0000;
            foreach (var b in buffer)
            {
                crc = (ushort)(crc ^ (b));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                }
            }
            return (ushort)(crc ^ 0xFFFF);
        }
        public ushort CRC16_USB(byte[] buffer)
        {
            ushort crc = 0xFFFF;
            foreach (var b in buffer)
            {
                crc = (ushort)(crc ^ (b));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                }
            }
            return (ushort)(crc ^ 0xFFFF);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blk_adr">输入要校验的字节数组</param>
        /// <param name="ulPoly">Poly值，如果是CRC16，那么就是0x8005，如果是CRC-CCITT，那么就是0x1201,这些值网上可以查到</param>
        /// <param name="ulInit">CRC初始值，这个很重要，一般我们都是用0作为初始值，但是你的例子中是用0xffff作为初始值，你之所以得到不同的结果，就是这个CRC初始值作怪，询问下对方，那个CRC初始值是多少。</param>
        /// <param name="ulXorOut">取反输出设置，用在加密上，一般设置0即可。</param>
        /// <param name="ulMask">标志位，如果你你是CRC16，就用0xFFFF，输出16位校验码，如果是CRC8，就用0xFF，输出8位校验码。</param>
        /// <returns></returns>
        private ulong CRC_any(byte[] blk_adr, ulong ulPoly, ulong ulInit, ulong ulXorOut, ulong ulMask)
        {
            ulong crc = ulInit;
            byte ucByte;
            int blk_len = blk_adr.Length;
            int i;
            bool iTopBitCRC;
            bool iTopBitByte;
            ulong ulTopBit;
            if (ulMask > 0xffff)
                ulTopBit = 0x80000000;
            else
                ulTopBit = ((ulMask + 1) >> 1);

            for (int j = 0; j < blk_len; j++)
            {
                ucByte = blk_adr[j];
                for (i = 0; i < 8; i++)
                {
                    iTopBitCRC = (crc & ulTopBit) != 0;
                    iTopBitByte = (ucByte & 0x80) != 0;
                    if (iTopBitCRC != iTopBitByte)
                    {
                        crc = (crc << 1) ^ ulPoly;
                    }
                    else
                    {
                        crc = (crc << 1);
                    }
                    ucByte <<= 1;
                }
            }
            return (ulong)((crc ^ ulXorOut) & ulMask);
        }
    }
}
