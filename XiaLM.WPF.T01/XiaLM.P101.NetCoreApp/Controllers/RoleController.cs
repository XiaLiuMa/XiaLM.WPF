﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XiaLM.P101.NetCoreApp.Controllers;
using XiaLM.P101.NetCoreService.Dtos;
using XiaLM.P101.NetCoreService.IService;

namespace XiaLM.NetCoreT02.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _service;
        public RoleController(IRoleService service)
        {
            _service = service;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增或编辑功能
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IActionResult Edit(RoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = GetModelStateError()
                });
            }
            if (dto.Id == Guid.Empty)
                dto.CreateTime = DateTime.Now;
            //dto.CreateUserId = 
            if (_service.InsertOrUpdate(dto))
            {
                return Json(new { Result = "Success" });
            }
            return Json(new { Result = "Faild" });
        }

        public IActionResult GetAllPageList(int startPage, int pageSize)
        {
            int rowCount = 0;
            var result = _service.GetAllPageList(startPage, pageSize, out rowCount);
            return Json(new
            {
                rowCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize),
                rows = result,
            });
        }
        public IActionResult DeleteMuti(string ids)
        {
            try
            {
                string[] idArray = ids.Split(',');
                List<Guid> delIds = new List<Guid>();
                foreach (string id in idArray)
                {
                    delIds.Add(Guid.Parse(id));
                }
                _service.DeleteBatch(delIds);
                return Json(new
                {
                    Result = "Success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = ex.Message
                });
            }
        }
        public IActionResult Delete(Guid id)
        {
            try
            {
                _service.Delete(id);
                return Json(new
                {
                    Result = "Success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = ex.Message
                });
            }
        }
        public IActionResult Get(Guid id)
        {
            var dto = _service.Get(id);
            return Json(dto);
        }

        /// <summary>
        /// 根据角色获取权限
        /// </summary>
        /// <returns></returns>
        public IActionResult GetMenusByRole(Guid roleId)
        {
            var dtos = _service.GetAllMenuListByRole(roleId);
            return Json(dtos);
        }

        public IActionResult SavePermission(Guid roleId, List<RoleMenuDto> roleMenus)
        {
            if (_service.UpdateRoleMenu(roleId, roleMenus))
            {
                return Json(new { Result = "Success" });
            }
            return Json(new { Result = "Faild" });
        }
    }
}