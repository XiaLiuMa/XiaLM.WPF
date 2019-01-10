using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XiaLM.P101.NetCoreDb.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

    }
}
