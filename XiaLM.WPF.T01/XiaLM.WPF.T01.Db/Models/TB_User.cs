using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.WPF.T01.Db.Models
{
    public class TB_User
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string SignTime { get; set; }
        /// <summary>
        /// 是否免登陆
        /// </summary>
        public bool IsAutoLogin { get; set; }
        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool IsRemember { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        public string LoginTime { get; set; }
    }
}
