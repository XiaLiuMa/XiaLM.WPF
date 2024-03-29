﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using XiaLM.P101.Quartz.Db.Entities;
using XiaLM.P101.Quartz.Scheduler;
using XiaLM.P101.Quartz.Scheduler.Models;

namespace XiaLM.P101.Quartz
{
    public class BaseMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ScheduleEntity, Schedule>();
                cfg.CreateMap<Schedule, ScheduleEntity>();
            });
        }
    }
}
