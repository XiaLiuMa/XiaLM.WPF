using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XiaLM.P101.NetCoreService.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XiaLM.P101.NetCoreApp.Components
{
    [ViewComponent(Name = "Navigation")]
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly IUserService _userService;
        public NavigationViewComponent(IMenuService menuService, IUserService userService)
        {
            _menuService = menuService;
            _userService = userService;
        }

        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.Session.GetString("CurrentUserId");
            var menus = _menuService.GetMenusByUser(Guid.Parse(userId));
            return View(menus);
        }
    }
}
