using XiaLM.P101.NetCoreDb.Entities;
using XiaLM.P101.NetCoreDb.IRepositories;

namespace XiaLM.P101.NetCoreDb.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(BaseDBContext dbcontext) : base(dbcontext)
        {

        }
    }
}
