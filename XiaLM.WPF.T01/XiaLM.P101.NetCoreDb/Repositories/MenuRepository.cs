using XiaLM.P101.NetCoreDb.Entities;
using XiaLM.P101.NetCoreDb.IRepositories;

namespace XiaLM.P101.NetCoreDb.Repositories
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(BaseDBContext dbcontext) : base(dbcontext)
        {

        }
    }
}
