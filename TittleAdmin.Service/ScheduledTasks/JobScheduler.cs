using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Service.ScheduledTasks
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            //push notification
            IJobDetail job = JobBuilder.Create<PushNotificationJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule
                  (s =>
                     s.WithIntervalInMinutes(5)
                    .RepeatForever())
                .Build();

            //reminder notification
            IJobDetail job2 = JobBuilder.Create<ReminderNotificationJob>().Build();

            ITrigger trigger2 = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.StartingDailyAt(new TimeOfDay(7,0))
                    .OnEveryDay())
                .Build();

            scheduler.ScheduleJob(job, trigger);
            scheduler.ScheduleJob(job2, trigger2);
        }
    }
}
