using MaxRobotServerApp.Mods;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals
{
    public class PreachresourceDal
    {
        public ResourceInfo Query(int id)
        {
            var  management = new ResourceManagement();
            return management.Query(id);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public OutPage<ResourceInfo> GetPreachresources(int currentPage, int pageSize)
        {
            #region 模拟数据
            //Faker<Preachresource> generator = new Faker<Preachresource>()
            //        .StrictMode(true)
            //        .RuleFor(x => x.ID, f => f.IndexGlobal)
            //        .RuleFor(x => x.Path, f => f.Person.FirstName)
            //        .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            //        .RuleFor(x => x.LastName, f => f.Person.LastName)
            //        .RuleFor(x => x.DOB, f => f.Person.DateOfBirth);
            //return generator.Generate(300); 
            #endregion
            ResourceManagement management = new ResourceManagement();
            OutPage<ResourceInfo> outPage = management.GetResources(new InPage()
            {
                Current = currentPage,
                Row = pageSize
            });
            return outPage;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public OutPage<ResourceInfo> GetPreachresources(int currentPage, int pageSize, ResourceType type)
        {
            ResourceManagement management = new ResourceManagement();
            OutPage<ResourceInfo> outPage = management.GetResources(new InPage()
            {
                Current = currentPage,
                Row = pageSize
            }, type);
            return outPage;
        }

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(int[] ids)
        {
            ResourceManagement management = new ResourceManagement();
            return management.Delete(ids);
        }
    }
}
