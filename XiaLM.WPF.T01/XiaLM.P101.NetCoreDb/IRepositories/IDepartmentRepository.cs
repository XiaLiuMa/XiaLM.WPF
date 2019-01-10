using System;
using XiaLM.P101.NetCoreDb.Entities;

namespace XiaLM.P101.NetCoreDb.IRepositories
{
    public interface IDepartmentRepository : IBaseRepository<Department, Guid>
    {
    }
}
