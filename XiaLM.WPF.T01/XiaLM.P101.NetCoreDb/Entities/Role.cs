using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XiaLM.P101.NetCoreDb.Entities
{
    public class Role : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Guid CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Remarks { get; set; }

        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
