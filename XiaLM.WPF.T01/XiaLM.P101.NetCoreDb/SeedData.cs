﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiaLM.P101.NetCoreDb.Entities;

namespace XiaLM.P101.NetCoreDb
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BaseDBContext(serviceProvider.GetRequiredService<DbContextOptions<BaseDBContext>>()))
            {
                if (context.Users.Any())
                {
                    return;   // 已经初始化过数据，直接返回
                }
                Guid departmentId = Guid.NewGuid();
                //增加一个部门
                context.Departments.Add(
                   new Department
                   {
                       Id = departmentId,
                       Name = "Fonour集团总部",
                       ParentId = Guid.Empty
                   }
                );
                //增加一个超级管理员用户
                context.Users.Add(
                     new User
                     {
                         UserName = "admin",
                         Password = "123456", //暂不进行加密
                         Name = "超级管理员",
                         DepartmentId = departmentId
                     }
                );
                //增加四个基本功能菜单
                context.Menus.AddRange(
                   new Menu
                   {
                       Name = "组织机构管理",
                       Code = "Department",
                       SerialNumber = 0,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "角色管理",
                       Code = "Role",
                       SerialNumber = 1,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "用户管理",
                       Code = "User",
                       SerialNumber = 2,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   },
                   new Menu
                   {
                       Name = "功能管理",
                       Code = "Department",
                       SerialNumber = 3,
                       ParentId = Guid.Empty,
                       Icon = "fa fa-link"
                   }
                );
                context.SaveChanges();
            }
        }
    }
}
