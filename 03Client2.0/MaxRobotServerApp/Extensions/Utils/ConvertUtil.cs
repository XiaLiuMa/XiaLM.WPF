using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Extensions.Utils
{
    public class ConvertUtil
    {
        /// <summary>
        /// Bitmap转byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] Bitmap2Bytes(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
    }
}
