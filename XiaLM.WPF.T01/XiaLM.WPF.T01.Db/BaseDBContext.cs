using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.WPF.T01.Db.Models;

namespace XiaLM.WPF.T01.Db
{
    public class BaseDBContext : DbContext
    {
        public BaseDBContext() : base("Db_WpfT01") { }
        /// <summary>
        /// 用户列表
        /// </summary>
        public DbSet<TB_User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
