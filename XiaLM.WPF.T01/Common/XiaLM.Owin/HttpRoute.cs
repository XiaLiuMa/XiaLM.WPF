using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace XiaLM.Owin
{
    public class HttpRoute
    {
        private string name = "DefaultApi";
        /// <summary>
        /// 路由名称
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        private string routeTemplate = @"api/{controller}/{id}";
        /// <summary>
        /// 路由模板
        /// </summary>
        public string RouteTemplate { get { return routeTemplate; } set { routeTemplate = value; } }
        private object _defaults = new { id = RouteParameter.Optional };

        /// <summary>
        /// 默认值
        /// </summary>
        public object Defaults { get { return _defaults; } set { _defaults = value; } }

    }
}
