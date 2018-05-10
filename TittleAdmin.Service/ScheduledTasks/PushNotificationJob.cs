using Quartz;
using System;
using System.Collections.Generic;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;
using TittleAdmin.Service.FCM;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Service.ScheduledTasks
{
    public class PushNotificationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            PushNotification fcm = new PushNotification();
            TittleNotificationServices service = new TittleNotificationServices();
            //get all the active notifications
            List<notification> notifications = service.GetActiveNotifications();
            //loop through notifications
            if (notifications != null)
            {
                for (int i = 0; i < notifications.Count; i++)
                {
                    string message = notifications[i].content;
                    //get list of users in notification
                    List<CustomNotificationUser> users = service.GetNotificationUsersList(notifications[i].id);
                    if (users != null && users.Count > 0)
                    {
                        //loop through users
                        for(int j = 0; j < users.Count; j++)
                        {
                            //save in notification box
                            notification_boxes nb = new notification_boxes();
                            nb.type = 10;
                            nb.device_id = users[j].id;
                            nb.device_type = "App\\Models\\User";
                            nb.message = message;
                            nb.task_id = 0;
                            nb.unread = "";
                            nb.seen = 0;
                            nb.kid_id_ref = 0;
                            service.SaveNotificationBoxInfo(nb);
                            //send notification one by one to devices
                            List<device> devices = service.GetListOfDevices(users[j].id);
                            for(int k=0;k<devices.Count; k++)
                            {
                                fcm.Send(10, message, devices[k].id);
                            }
                        }
                    }
                    //update notification table
                    if (notifications[i].type == "onetime")
                        notifications[i].status = "completed";
                    else
                    {
                        if (notifications[i].type == "daily")
                        {
                            notifications[i].next_notification = ((DateTime)notifications[i].next_notification).AddDays(1);
                        }
                        else if (notifications[i].type == "weekly")
                        {
                            notifications[i].next_notification = ((DateTime)notifications[i].next_notification).AddDays(7);
                        }
                        else if (notifications[i].type == "monthly")
                        {
                            notifications[i].next_notification = ((DateTime)notifications[i].next_notification).AddMonths(1);
                        }
                    }
                    service.UpdateNotificationInfo(notifications[i]);
                }
            }
        }
    }
}
