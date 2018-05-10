using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;
using TittleAdmin.Models;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class UserTasksController : Controller
    {
        [Route("user-tasks")]
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
        public JsonResult UserTasks(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchUserTasks(model);

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
        public DataTableResult<CustomPerkTask> SearchUserTasks(DataTableAjaxPostModel model)
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
            TittleUserServices _Service = new TittleUserServices();
            DataTableResult<CustomPerkTask> result = new Models.DataTableResult<CustomPerkTask>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            result.result = _Service.GetPerkTasks(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomPerkTask>();
            }
            return result;
        }

        [HttpGet]
        public JsonResult IsPerkTaskExist(string key, long id)
        {
            TittleUserServices service = new TittleUserServices();
            bool isExist = false;
            perks_tasks data = service.GetPerkTaskByKey(key);
            if (data == null)
                isExist = false;
            else if (data.id == id)
                isExist = false;
            else
                isExist = true;

            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(CustomPerkTask model)
        {
            string sMessage = "";
            if (ModelState.IsValid)
            {
                TittleUserServices service = new TittleUserServices();
                long nID = 0;
                string filename = "";
                if(model.image!=null && model.image.ContentLength > 0)
                {
                    string websiteUrl = ConfigurationManager.AppSettings["WebsiteUrl"].ToString();
                    string filePath = ConfigurationManager.AppSettings["TaskIconPath"].ToString();
                    filename = RandomString(15) + Path.GetExtension(model.image.FileName);
                    string fullPath = Server.MapPath(filePath + "/" + filename);
                    if (!Directory.Exists(Server.MapPath(filePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(filePath));
                    }
                    model.image.SaveAs(fullPath);
                    model.icon = websiteUrl + filePath + "/" + filename;
                }
                sMessage = service.SaveOrUpdatePerkTask(model, ref nID);

            }

            return Json(new
            {
                message = sMessage
            });
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult AddPerkTask(int Id)
        {
            TittleUserServices service = new TittleUserServices();

            CustomPerkTask model = new CustomPerkTask();

            if (Id > 0)
            {
                perks_tasks _data = service.GetPerkTaskInfo(Id);
                model.key = _data.key;
                model.name = _data.name;
                model.number_to_finish = _data.number_to_finish;
                model.score = _data.score;
                model.icon = _data.icon;
                model.id = _data.id;
            }
            return PartialView("AddPerkTask", model);
        }

        [HttpPost]
        public ActionResult DeletePerkTask(int Id)
        {
            TittleUserServices service = new TittleUserServices();
            string sMessage = "";
            if (Id > 0)
            {
                service.DeletePerkTask(Id, ref sMessage);
            }
            return Json(new
            {
                message = sMessage
            });
        }
    }
}