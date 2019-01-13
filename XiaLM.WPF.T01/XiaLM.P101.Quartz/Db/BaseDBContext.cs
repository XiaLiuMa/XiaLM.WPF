using Microsoft.EntityFrameworkCore;
using XiaLM.P101.Quartz.Db.Entities;

namespace XiaLM.P101.Quartz.Db
{
    public class BaseDBContext : DbContext
    {
        public BaseDBContext(DbContextOptions<BaseDBContext> options) : base(options)
        {

        }
        public DbSet<Tb_Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });//UserRole关联配置
            base.OnModelCreating(builder);
        }
    }
}
