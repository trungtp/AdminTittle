using Quartz;
using System;
using TittleAdmin.Model.Model;
using TittleAdmin.Service.FCM;

namespace TittleAdmin.Service.ScheduledTasks
{
    public class ReminderNotificationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            PushNotification fcm = new PushNotification();
            //get list of users

            //loop through users
                //remove expired
                //get existing devices
                    //
        }

        public string getMessage(slot _slot, int diffInDay)
        {
            string retVal = "";
            string msg = "";
            if (diffInDay == 0) {

                msg = "today";

            }
            else if (diffInDay == 3) {

                msg = "in 3 days";

            }
            else if (diffInDay == 7) {

                msg = "in 7 days";

            }

            if (_slot.type == "manage") {

                retVal = "Your subscription for Tittler slot #" + _slot.position + " expires " + msg + ". Please extend your subscription to continue managing your Tittler.";

            } else if (_slot.type == "access") {

                retVal = "Your subscription for Parent slot #" + _slot.position + " expires " + msg + ". Please extend your subscription to continue accessing your account.";

            }

            return retVal;

        }
    }
}
