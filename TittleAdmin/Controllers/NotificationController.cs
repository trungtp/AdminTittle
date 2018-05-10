using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;
using TittleAdmin.Models;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public JsonResult Notifications(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchNotifications(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result
            });
        }

        [NonAction]
        public DataTableResult<CustomNotification> SearchNotifications(DataTableAjaxPostModel model)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            TittleNotificationServices _Notificationservice = new TittleNotificationServices();
            DataTableResult<CustomNotification> result = new Models.DataTableResult<CustomNotification>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            result.result = _Notificationservice.GetNotificationList(searchBy, take, skip, sortBy, sortDir,model.CustomData, out filteredResultsCount, out totalResultsCount);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomNotification>();
            }
            return result;
        }

        public ActionResult UpdateNotification(int Id)
        {
            TittleNotificationServices notificationService = new TittleNotificationServices();

            CustomNotification model = new CustomNotification();

            if (Id > 0)
            {
                notification notifcation = notificationService.GetNotificationInfo(Id);
                model.name = notifcation.name;
                model.content = notifcation.content;
                model.OnDate = Convert.ToDateTime(notifcation.time).ToString("dd/MM/yyyy hh:mm tt");
                if (notifcation.status.ToLower() == "draft")
                {
                    model.notificationStatus = NotificationStatus.Draft;
                }
                else
                {
                    model.notificationStatus = NotificationStatus.Published;
                }
                switch (notifcation.type.ToLower())
                {
                    case "onetime":
                        model.notificationType = NotificationTypes.OneTime;
                        break;
                    case "daily":
                        model.notificationType = NotificationTypes.Daily;
                        break;
                    case "weekly":
                        model.notificationType = NotificationTypes.Weekly;
                        break;
                    case "monthly":
                        model.notificationType = NotificationTypes.Monthly;
                        break;
                    default:
                        model.notificationType = NotificationTypes.Daily;
                        break;
                }
            }
            else
            {
                model.notificationType = NotificationTypes.Daily;
                model.notificationStatus = NotificationStatus.Draft;
            }

            return PartialView("EditNotification", model);
        }

        [HttpPost]
        public ActionResult Index(CustomNotification model)
        {
            string sMessage = "";
            if (ModelState.IsValid)
            {
                TittleNotificationServices notificationService = new TittleNotificationServices();
                long nID = 0;
                model.status = model.notificationStatus.ToString().ToLower();
                model.type = model.notificationType.ToString().ToLower();

                sMessage = notificationService.SaveOrUpdateNotification(model, ref nID);
            }

            return Json(new
            {
                message = sMessage
            });
        }
        
        [HttpPost]
        public ActionResult DeleteNotification(int Id)
        {
            TittleNotificationServices notificationService = new TittleNotificationServices();
            string sMessage = "";
            if (Id > 0)
            {
                notificationService.DeleteNotification(Id, ref sMessage);
            }
            return Json(new
            {
                message = sMessage
            });
        }

        [HttpPost]
        public ActionResult PublishNotification(int Id)
        {
            TittleNotificationServices notificationService = new TittleNotificationServices();
            string sMessage = "";
            if (Id > 0)
            {
                notification nt = notificationService.GetNotificationInfo(Id);
                nt.status = NotificationStatus.Published.ToString().ToLower();
                notificationService.UpdateNotificationInfo(nt);
            }
            return Json(new
            {
                message = sMessage
            });
        }

        public ActionResult GetNotificationUsers(int Id)
        {
            TittleNotificationServices notService = new TittleNotificationServices();
            List<CustomNotificationUser> model = notService.GetNotificationUsersList(Id);
            return PartialView("NotificationUser", model);
        }
    }
}