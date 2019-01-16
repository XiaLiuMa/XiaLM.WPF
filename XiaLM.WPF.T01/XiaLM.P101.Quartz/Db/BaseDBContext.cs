using Microsoft.EntityFrameworkCore;
using XiaLM.P101.Quartz.Db.Entities;

namespace XiaLM.P101.Quartz.Db
{
    public class BaseDBContext : DbContext
    {
        private readonly static object objLock = new object();
        private static BaseDBContext instance = null;
        public static BaseDBContext GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new BaseDBContext();
                    }
                }
            }
            return instance;
        }

        //public BaseDBContext(DbContextOptions<BaseDBContext> options) : base(options)
        //{

        //}
        public DbSet<ScheduleEntity> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });//UserRole关联配置
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //注入Sql链接字符串
            optionsBuilder.UseSqlServer(@"Server=localhost;User Id=sa;Password=123456;Database=Db_Quartz");
        }
    }
}
