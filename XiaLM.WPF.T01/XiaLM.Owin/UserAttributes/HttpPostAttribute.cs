using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace XiaLM.Owin.UserAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public  class HttpPostAttribute : Attribute, System.Web.Http.Controllers.IActionHttpMethodProvider
    {
        System.Web.Http.HttpPostAttribute httpAttribute;
        public HttpPostAttribute()
        {
            httpAttribute = new System.Web.Http.HttpPostAttribute();
        }
        public Collection<HttpMethod> HttpMethods => httpAttribute.HttpMethods;
    }
}
