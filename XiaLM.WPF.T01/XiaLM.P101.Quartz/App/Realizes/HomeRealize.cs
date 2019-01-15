using System;
using System.Collections.Generic;
using System.Text;
using XiaLM.P101.Quartz.Db.IManaments;

namespace XiaLM.P101.Quartz.App.Realizes
{
    public class HomeRealize
    {
        public IScheduleManament manament { get; set; }

        public HomeRealize(IScheduleManament manament)
        {
            this.manament = manament;
        }

        public void fu()
        {

        }
        
    }
}
