using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;
using TittleAdmin.Models;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
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
        public JsonResult Users(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchUsers(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchUsers(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetUsersList(searchBy, model.sort_by, model.start_time, model.end_time, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
        [HttpPost]
        public JsonResult UsersByCountry(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchUsersByCountry(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchUsersByCountry(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetUsersByCountryList(searchBy, model.sort_by, model.start_time, model.end_time, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
        [HttpPost]
        public JsonResult ActiveUsers(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchActiveUsers(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchActiveUsers(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetActiveUsersList(searchBy, model.sort_by, model.start_time, model.end_time, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
        [HttpPost]
        public JsonResult InAppUsers(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchInAppUsers(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchInAppUsers(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetInAppPurchaseUsersList(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
        [HttpPost]
        public JsonResult GrossUsers(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchGrossUsers(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchGrossUsers(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetGrossAmountUsersList(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
        [HttpPost]
        public JsonResult TotalAmountUsers(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchTotalAmountUsers(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchTotalAmountUsers(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetTotalAmountUsersList(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
        [HttpPost]
        public JsonResult PromoCodeUsers(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchPromoCodeUsers(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchPromoCodeUsers(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetPromoUsedUsersList(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
        [HttpPost]
        public JsonResult PerksUsers(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchPerksUsers(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchPerksUsers(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetPerksUsedUsersList(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
        [HttpPost]
        public JsonResult RedeemUsers(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchRedeemUsers(model);

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = result.totalResultsCount,
                recordsFiltered = result.filteredResultsCount,
                data = result.result,
                ios = result.iosCount,
                android = result.androidCount,
                country = result.countryGrouping
            });
        }

        [NonAction]
        public DataTableResult<CustomUser> SearchRedeemUsers(DataTableAjaxPostModel model)
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
            TittleUserServices _UserServices = new TittleUserServices();
            DataTableResult<CustomUser> result = new Models.DataTableResult<CustomUser>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            int iosCount = 0;
            int androidCount = 0;
            string countryGrouping = "";
            result.result = _UserServices.GetRedeemUsedUsersList(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, out iosCount, out androidCount, out countryGrouping);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            result.iosCount = iosCount;
            result.androidCount = androidCount;
            result.countryGrouping = countryGrouping;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUser>();
            }
            return result;
        }
    }
}