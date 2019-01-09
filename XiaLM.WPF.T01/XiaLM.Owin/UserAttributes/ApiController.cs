using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Owin.UserAttributes
{
    /// <summary>
    /// webapi控制类需要继承，
    /// 需要引用\packages\Microsoft.AspNet.WebApi.Core.5.2.3\lib\net45\System.Web.Http.dll
    /// </summary>
    public abstract partial class  ApiController: System.Web.Http.ApiController
    {
    }
}
