using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Data;

namespace XiaLM.Util
{
    public static class JsonUtil
    {
        /// <summary>
        /// 对象转json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T StrToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// json字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ObjectToStr<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }


        /// <summary>
        /// DataTable转对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> DataTableToObjs<T>(this DataTable dataTable)
        {
            string jsonString = JsonConvert.SerializeObject(dataTable, new DataTableConverter());
            return JsonConvert.DeserializeObject<List<T>>(jsonString);
        }

        /// <summary>
        /// 对象列表转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ObjsToDataTable<T>(this List<T> list)
        {
            string jsonString = JsonConvert.SerializeObject(list);
            return JsonConvert.DeserializeObject<DataTable>(jsonString, new DataTableConverter());
        }
    }
}
