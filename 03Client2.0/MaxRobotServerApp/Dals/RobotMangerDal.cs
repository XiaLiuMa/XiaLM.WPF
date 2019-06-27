using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals
{
    public class RobotMangerDal
    {
        /// <summary>
        /// 获取机器人列表
        /// </summary>
        /// <returns>返回查询列表</returns>
        public List<RobotInfo> GetRobotList()
        {
            RobotManagement management = new RobotManagement();
            OutPage<RobotInfo>  outdata = management.GetRobotList(new InPage());
            return outdata.Datas;
        }

        /// <summary>
        /// 得到机器人信息
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public RobotInfo GetRobotInfo(string tag)
        {
            RobotManagement management = new RobotManagement();
            return management.GetRobotInfo(tag);
        }

        /// <summary>
        /// 修改机器人信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns>是否成功</returns>
        public bool UpdataRobotInfo(RobotInfo info)
        {
            RobotManagement management = new RobotManagement();
            return management.UpdataRobotInfo(info);
        }

        /// <summary>
        /// 删除机器人信息
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>是否成功</returns>
        public bool DeleteRobotInfo(string tag)
        {
            RobotManagement management = new RobotManagement();
            return management.DeleteRobotInfo(tag);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="cmd"></param>
        /// <returns>是否成功</returns>
        public bool SendCmd(string tag, string cmd)
        {
            RobotManagement management = new RobotManagement();
            return management.SendCmd(tag, cmd);
        }
    }
}
