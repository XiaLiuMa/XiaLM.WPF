using System;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace XiaLM.Owin.UserAttributes
{
    public class HttpGetAttribute : Attribute,System.Web.Http.Controllers.IActionHttpMethodProvider
    {
        System.Web.Http.HttpGetAttribute httpAttribute;
        public HttpGetAttribute()
        {
            httpAttribute = new System.Web.Http.HttpGetAttribute();
        }
        public Collection<HttpMethod> HttpMethods => httpAttribute.HttpMethods;
    }
}
