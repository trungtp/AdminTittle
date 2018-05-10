using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Model.DTO;
using TittleAdmin.Models;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class KpiReportController : Controller
    {
        [Route("user-dashboard")]
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View("Dashboard");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [Route("user-plans")]
        public ActionResult Plans()
        {
            if (Session["UserID"] != null)
            {
                return View("Plans");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public JsonResult UserPlans(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchUserPlans(model);

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
        public DataTableResult<CustomUserPlan> SearchUserPlans(DataTableAjaxPostModel model)
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
            DataTableResult<CustomUserPlan> result = new Models.DataTableResult<CustomUserPlan>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            result.result = _Service.GetUserPlans(searchBy, model.promoCode, model.plan, model.fromDate, model.toDate, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUserPlan>();
            }
            return result;
        }

        [Route("user-actions")]
        public ActionResult Actions()
        {
            if (Session["UserID"] != null)
            {
                return View("Actions");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public JsonResult UserActions(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchUserActions(model);

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
        public DataTableResult<CustomUserAction> SearchUserActions(DataTableAjaxPostModel model)
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
            DataTableResult<CustomUserAction> result = new Models.DataTableResult<CustomUserAction>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            result.result = _Service.GetUserActions(searchBy,model.action, model.fromDate,model.toDate, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUserAction>();
            }
            return result;
        }
    }
}