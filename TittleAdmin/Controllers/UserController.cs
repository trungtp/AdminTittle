using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;
using TittleAdmin.Service.FCM;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult ResetPassword(int id)
        {
            if (Session["UserID"] != null)
            {
                TittleUserServices service = new TittleUserServices();
                CustomResetUser model = service.GetUserDetail(id);
                return PartialView("ResetPassword", model);
            }
            return null;
        }

        [HttpPost]
        public ActionResult UserActive(CustomResetUser model)
        {
            if (Session["UserID"] != null)
            {
                TittleUserServices service = new TittleUserServices();
                service.ChangeUserActiveStatus(model);

                return Json(new
                {
                    message = "success"
                });
            }
            return Json(new
            {
                message = "error"
            });
        }

        public ActionResult UserInfo(int id)
        {
            if (Session["UserID"] != null)
            {
                TittleUserServices service = new TittleUserServices();
                CustomUserInfo model = service.GetUserInformations(id);
                return PartialView("UserInfo", model);
            }
            return null;
        }

        // GET: SendEmail
        [HttpPost]
        public ActionResult SendEmail(List<string> obj)
        {
            if (Session["UserID"] != null)
            {
                CustomNotify model = new CustomNotify();
                model.ToEmails = obj;
                if(obj!=null)
                    model.To = string.Join("|", obj.ToArray());
                return PartialView("SendEmail", model);
            }
            return null;
        }

        [HttpPost]
        public ActionResult SendEmails(CustomNotify model)
        {
            if (Session["UserID"] != null)
            {
                TittleUserServices service = new TittleUserServices();
                List<string> emails = null;
                if (!string.IsNullOrEmpty(model.To))
                    emails = model.To.Split('|').ToList();
                else
                    emails = service.GetAllUsersEmail();

                //loop through users and send emails
                foreach(string email in emails)
                {
                    MailData mail = new MailData();
                    mail.Body = model.Content;
                    mail.Subject = model.Subject;
                    mail.To = email;
                    MailService.SendEmail(mail);
                }

                return Json(new
                {
                    message = "success"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                message = "error"
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: PushNotification
        [HttpPost]
        public ActionResult PushNotification(List<string> obj, List<long> ids)
        {
            if (Session["UserID"] != null)
            {
                CustomNotify model = new CustomNotify();
                model.ToEmails = obj;
                if (obj != null && obj.Count > 0)
                    model.To = string.Join("|", obj.ToArray());
                return PartialView("PushNotification", model);
            }
            return null;
        }

        [HttpPost]
        public ActionResult PushNotifications(CustomNotify model)
        {
            if (Session["UserID"] != null && ModelState.IsValid)
            {
                PushNotification fcm = new PushNotification();
                TittleUserServices service = new TittleUserServices();
                TittleNotificationServices nservice = new TittleNotificationServices();
                List<string> emails = null;

                if(model.Type == "immediately")
                {
                    if (!string.IsNullOrEmpty(model.To))
                        emails = model.To.Split('|').ToList();
                    else
                        emails = service.GetAllUsersEmail();
                    //loop through users and send notification
                    foreach (string email in emails)
                    {
                        List<CustomNotificationUser> users = nservice.GetUserDetailByEmail(email);
                        for (int j = 0; j < users.Count; j++)
                        {
                            //save in notification box
                            notification_boxes nb = new notification_boxes();
                            nb.type = 10;
                            nb.device_id = users[j].id;
                            nb.device_type = "App\\Models\\User";
                            nb.message = model.Content;
                            nb.task_id = 0;
                            nb.unread = "";
                            nb.seen = 0;
                            nb.kid_id_ref = 0;
                            nservice.SaveNotificationBoxInfo(nb);
                            //send notification one by one to devices
                            List<device> devices = nservice.GetListOfDevices(users[j].id);
                            for (int k = 0; k < devices.Count; k++)
                            {
                                fcm.Send(10, model.Content, devices[k].id);
                            }
                        }
                    }
                }
                else
                {
                    CustomNotification not = new CustomNotification();
                    if (!string.IsNullOrEmpty(model.To))
                        not.data = "{\"users\":\"" + string.Join(",",model.To.Split('|')) + "\"}";
                    else
                        not.data = "{\"users\":\"all\"}";
                    not.content = model.Content;
                    not.name = string.IsNullOrEmpty(model.Name)? "Unknown": model.Name;
                    not.OnDate = model.TimeStart.HasValue ? model.TimeStart.Value.ToString("dd/MM/yyyy hh:mm tt") : null;
                    not.status = model.Status;
                    not.type = model.Type;
                    long nid = 0;
                    nservice.AddNotification(not, ref nid);
                }
                return Json(new
                {
                    message = "success"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                message = "error"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ReminderSetting()
        {
            if (Session["UserID"] != null)
            {
                TittleNotificationServices service = new TittleNotificationServices();
                system_settings model = service.GetSystemNotificationInfo();
                return PartialView("ChangeReminderSetting", model);
            }
            return null;
        }
        [HttpGet]
        public ActionResult ChangeNotificationSetting(string value)
        {
            if (Session["UserID"] != null)
            {
                TittleNotificationServices service = new TittleNotificationServices();
                service.UpdateSystemNotificationInfo(value);
                return Json(new
                {
                    message = "success"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                message = "error"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}