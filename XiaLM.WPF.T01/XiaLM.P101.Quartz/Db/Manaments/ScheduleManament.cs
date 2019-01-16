using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiaLM.P101.Quartz.Db.Entities;
using XiaLM.P101.Quartz.Db.IManaments;

namespace XiaLM.P101.Quartz.Db.Manaments
{
    public class ScheduleManament : BaseManament<ScheduleEntity, Guid>, IScheduleManament
    {
        public ScheduleManament(BaseDBContext dbcontext) : base(dbcontext)
        {
        }

    }
}
