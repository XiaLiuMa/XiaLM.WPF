using Microsoft.EntityFrameworkCore;
using XiaLM.P101.NetCoreDb.Entities;

namespace XiaLM.P101.NetCoreDb
{
    public class BaseDBContext : DbContext
    {
        public BaseDBContext(DbContextOptions<BaseDBContext> options) : base(options)
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });//UserRole关联配置
            builder.Entity<RoleMenu>().HasKey(rm => new { rm.RoleId, rm.MenuId });//RoleMenu关联配置
            //builder.HasPostgresExtension("uuid-ossp");//启用Guid主键类型扩展

            base.OnModelCreating(builder);
        }
    }
}
