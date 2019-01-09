using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XiaLM.Log.Help
{
    /// <summary>
    /// xml文件序列化助手
    /// </summary>
    public class XmlSerializeHelper
    {
        /// <summary>
        /// 加载xml文件成对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">xml文件路径</param>
        /// <returns></returns>
        public static T LoadXmlToObject<T>(string path) where T : new()
        {
            T t = new T();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string text = sr.ReadToEnd();

                    t = SerializeXmlToObject<T>(text);
                }
            }
            return t;
        }

        /// <summary>
        /// 保存对象到xml文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">对象</param>
        /// <param name="path">xml保存路径</param>
        /// <returns></returns>
        public static bool SaveObjectToXml<T>(T t, string path)
        {
            try
            {
                string text = SerializeObjectToXml<T>(t);
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8)) //保存地址
                {
                    sw.WriteLine(text);
                    sw.Flush();
                    sw.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 实体对象序列化成xml字符串(未指定编码的情况下默认UTF8)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string SerializeObjectToXml<T>(T obj)
        {
            return SerializeObjectToXml<T>(obj, Encoding.UTF8);
        }

        /// <summary>
        /// 实体对象序列化成xml字符串
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string SerializeObjectToXml<T>(T obj, Encoding encoding)
        {
            try
            {
                if (obj == null) throw new ArgumentNullException("obj");
                var ser = new XmlSerializer(obj.GetType());
                using (var ms = new MemoryStream())
                {
                    using (var writer = new XmlTextWriter(ms, encoding))
                    {
                        writer.Formatting = System.Xml.Formatting.Indented;
                        ser.Serialize(writer, obj);
                    }
                    var xml = encoding.GetString(ms.ToArray());
                    xml = xml.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                    xml = xml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                    return xml;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 反序列化xml字符为对象，默认为Utf-8编码
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xml">xml字符串</param>
        /// <returns></returns>
        public static T SerializeXmlToObject<T>(string xml) where T : new()
        {
            return SerializeXmlToObject<T>(xml, Encoding.UTF8);
        }

        /// <summary>
        /// 反序列化xml字符为对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xml">xml字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static T SerializeXmlToObject<T>(string xml, Encoding encoding) where T : new()
        {
            try
            {
                var mySerializer = new XmlSerializer(typeof(T));
                using (var ms = new MemoryStream(encoding.GetBytes(xml)))
                {
                    using (var sr = new StreamReader(ms, encoding))
                    {
                        return (T)mySerializer.Deserialize(sr);
                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
