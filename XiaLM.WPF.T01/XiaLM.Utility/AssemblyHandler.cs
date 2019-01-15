using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace XiaLM.Utility
{
    /// <summary>  
    /// 反射处理类  
    /// </summary>  
    public class AssemblyHandler
    {
        /// <summary>
        /// 获取(文件夹中)程序集名称列表
        /// </summary>
        /// <param name="dirPath">文件夹地址</param>
        /// <returns></returns>        
        public static List<string> GetAssemblyNames(string dirPath)
        {
            string[] dicFileName = Directory.GetFileSystemEntries(dirPath);
            if (dicFileName != null)
            {
                List<string> assemblyList = new List<string>();
                foreach (string name in dicFileName)
                {
                    assemblyList.Add(name.Substring(name.LastIndexOf('/') + 1));
                }
                return assemblyList;
            }
            return null;
        }

        /// <summary>  
        /// 获取程序集中的类名称列表  
        /// </summary>  
        /// <param name="assemblyName">程序集名(完整路径)</param>  
        public static List<string> GetClassNames(string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.LoadFrom(assemblyName);
                Type[] ts = assembly.GetTypes();
                List<string> classList = new List<string>();
                foreach (Type t in ts)
                {
                    classList.Add(t.FullName);
                }
                return classList;
            }
            return null;
        }

        /// <summary>  
        /// 获取类的属性列表  
        /// </summary>  
        /// <param name="assemblyName">程序集名(完整路径)</param>  
        /// <param name="className">类名(完整路径)</param>  
        public static List<string> GetClassAttributes(string assemblyName, string className)
        {
            if (!String.IsNullOrEmpty(assemblyName) && !String.IsNullOrEmpty(className))
            {
                Assembly assembly = Assembly.LoadFrom(assemblyName);
                Type type = assembly.GetType(className, true, true);
                if (type != null)
                {
                    //类的属性  
                    List<string> propertieList = new List<string>();
                    PropertyInfo[] propertyinfo = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    foreach (PropertyInfo p in propertyinfo)
                    {
                        propertieList.Add(p.ToString());
                    }
                    return propertieList;
                }
            }
            return null;
        }

        /// <summary>  
        /// 获取类的方法列表  
        /// </summary>  
        /// <param name="assemblyName">程序集(完整路径)</param>  
        /// <param name="className">类名(完整路径)</param>  
        public static List<string> GetClassMethods(string assemblyName, string className)
        {
            if (!String.IsNullOrEmpty(assemblyName) && !String.IsNullOrEmpty(className))
            {
                Assembly assembly = Assembly.LoadFrom(assemblyName);
                Type type = assembly.GetType(className, true, true);
                if (type != null)
                {
                    //类的方法  
                    List<string> methods = new List<string>();
                    MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (MethodInfo mi in methodInfos)
                    {
                        methods.Add(mi.Name);
                        //方法的参数  
                        //foreach (ParameterInfo p in mi.GetParameters())  
                        //{  

                        //}  
                        //方法的返回值  
                        //string returnParameter = mi.ReturnParameter.ToString();  
                    }
                    return methods;
                }
            }
            return null;
        }

        /// <summary>
        /// 反射获取DLL中的类的Type
        /// </summary>
        /// <param name="assemblyName">程序集名称(完整路径)</param>
        /// <param name="className">类名(完整路径)</param>
        /// <returns></returns>
        public static Type GetClassType(string assemblyName, string className)
        {
            Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));  // 获取指定程序集
            return assembly.GetType(className);
        }

        /// <summary>
        /// 反射获取前项目中的类的Type
        /// </summary>
        /// <param name="className">类名(完整路径)</param>
        /// <returns></returns>
        public static Type GetClassType(string className)
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
            return assembly.GetType(className);
        }

        /// <summary>
        /// 反射获取DLL中的类信息
        /// </summary>
        /// <param name="assemblyName">程序集名称(完整路径)</param>
        /// <param name="className">类名(完整路径)</param>
        /// <returns></returns>
        public static object GetClassObject(string assemblyName, string className)
        {
            Assembly assembly = Assembly.Load(new AssemblyName(assemblyName)); // 获取指定程序集
            return assembly.CreateInstance(className);
        }

        /// <summary>
        /// 反射获取前项目中的类信息
        /// </summary>
        /// <param name="className">类名(完整路径)</param>
        /// <returns></returns>
        public static object GetClassObject(string className)
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
            return assembly.CreateInstance(className);
        }
    }
}
