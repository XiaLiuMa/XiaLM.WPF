using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals
{
    public class SemanticmanagerDal
    {
        #region 语义库
        /// <summary>
        /// 得到语义库信息
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<SemanticGenreInfo> GetSemanticGenreInfos(int currentPage, int pageSize)
        {
            SemanticManagement management = new SemanticManagement();
            OutPage<SemanticGenreInfo> outPage = management.GetSemanticGenreInfos(new InPage()
            {
                Current = currentPage,
                Row = pageSize
            });
            return outPage;
        }

        /// <summary>
        /// 添加语义库
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool AddSemanticGenre(SemanticGenreInfo obj)
        {
            if (obj == null) return false;
            SemanticManagement management = new SemanticManagement();
            SemanticParms parms = new SemanticParms()
            {
                SemanticGenre = obj,
                SemanticInfos = new List<SemanticInfo>()
            };
            return management.AddSemanticInfo(parms);
        }


        /// <summary>
        /// 删除语义库
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteSemanticGenre(int[] ids)
        {
            SemanticManagement management = new SemanticManagement();
            return management.DeleteSemanticGenre(ids);
        }

        /// <summary>
        /// 修改语义库
        /// </summary>
        /// <param name="semanticGenre"></param>
        /// <returns></returns>
        public bool UpdataSemanticGenre(SemanticGenreInfo semanticGenre)
        {
            SemanticManagement management = new SemanticManagement();
            return management.UpdataSemanticGenre(semanticGenre);
        }
        #endregion




        /// <summary>
        /// 得到全部类型语义数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public OutPage<SemanticInfo> GetSemanticInfos(int currentPage, int pageSize)
        {
            SemanticManagement management = new SemanticManagement();
            OutPage<SemanticInfo> outPage = management.GetSemanticInfos(new InPage()
            {
                Current = currentPage,
                Row = pageSize
            });
            return outPage;
        }

        /// <summary>
        /// 根据语义库类型得到语义数据
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        public OutPage<SemanticInfo> GetSemanticInfos(string genre, int currentPage, int pageSize)
        {
            SemanticManagement management = new SemanticManagement();
            OutPage<SemanticInfo> outPage = management.GetSemanticInfos(genre, new InPage()
            {
                Current = currentPage,
                Row = pageSize
            });
            return outPage;
        }


        /// <summary>
        /// 删除语义信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteSemanticInfo(int[] ids)
        {
            SemanticManagement management = new SemanticManagement();
            return management.DeleteSemanticInfo(ids);
        }

        /// <summary>
        /// 添加语义数据
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="genreInfo"></param>
        /// <returns></returns>
        public bool AddSemanticInfo(List<SemanticInfo> lst, SemanticGenreInfo genreInfo)
        {
            if (lst == null || lst.Count <= 0 || genreInfo == null) return false;
            SemanticManagement management = new SemanticManagement();
            SemanticParms parms = new SemanticParms()
            {
                SemanticGenre = genreInfo,
                SemanticInfos = lst
            };
            return management.AddSemanticInfo(parms);
        }
    }
}
