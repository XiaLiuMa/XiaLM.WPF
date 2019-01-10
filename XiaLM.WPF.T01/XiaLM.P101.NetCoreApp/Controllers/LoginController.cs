using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XiaLM.P101.NetCoreApp.Models;
using XiaLM.P101.NetCoreService.IService;
using XiaLM.NetCoreUtility;

namespace XiaLM.NetCoreT02.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //检查用户信息
                var user = _userService.CheckUser(model.UserName, model.Password);
                if (user != null)
                {
                    //记录Session
                    HttpContext.Session.SetString("CurrentUserId", user.Id.ToString());
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                    ////跳转到系统首页
                    //return RedirectToAction("Index", "Home");
                    //跳转到系统首页
                    return RedirectToAction("Index", "Menu");
                }
                ViewBag.ErrorInfo = "用户名或密码错误。";
                return View();
            }
            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }
    }
}