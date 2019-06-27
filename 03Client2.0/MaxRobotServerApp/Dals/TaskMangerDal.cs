using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals
{
    public class TaskMangerDal
    {
        private RobotTaskManagement management;
        public TaskMangerDal()
        {
            management = new RobotTaskManagement();
        }
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <param name="mapid"></param>
        /// <returns></returns>
        public OutPage<TaskInfo> GetTasks(string query, InPage inPage)
        {
            return management.GetTasks(query, inPage);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool AddTask(TaskInfoParms task)
        {
            return management.AddTask(task);
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteTask(int[] ids)
        {
            return management.DeleteTask(ids);
        }
        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool UpdataTask(TaskInfoParms task)
        {
            return management.UpdataTask(task);
        }
        /// <summary>
        /// 根据任务id得到当前任务下的子任务列表
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        public List<RobotTask> GetTasksForTaskId(int taskid)
        {
            return management.GetTasksForTaskId(taskid);
        }

        /// <summary>
        /// 资源是否被使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public bool IsResourceUsed(int id)
        {
            return management.IsResourceUsed(id);
        }
        /// <summary>
        /// 线是否被使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public bool IsMapLineUsed(int id)
        {
            return management.IsMapLineUsed(id);
        }
        /// <summary>
        /// 点是都被使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public bool IsPositionUsed(int id)
        {
            return management.IsPositionUsed(id);
        }
        /// <summary>
        /// 地图是否被使用
        /// </summary>
        /// <param name="mapid"></param>
        /// <returns></returns>

        public bool IsMapUsed(string mapid)
        {
            return management.IsMapUsed(mapid);
        }
    }
}
