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
    public class RedeemController : Controller
    {
        [Route("user-redeems")]
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
        public JsonResult UserRedeems(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchRedeems(model);

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
        public DataTableResult<CustomUserRedeem> SearchRedeems(DataTableAjaxPostModel model)
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
            TittleRedeemServices _Service = new TittleRedeemServices();
            DataTableResult<CustomUserRedeem> result = new DataTableResult<CustomUserRedeem>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            result.result = _Service.GetUserRedeemList(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomUserRedeem>();
            }
            return result;
        }
        [HttpPost]
        public ActionResult ChangeStatus(long Id, string status)
        {
            TittleRedeemServices redeemServices = new TittleRedeemServices();
            string sMessage = "";
            if (Id > 0)
            {
                sMessage = redeemServices.ChangeRedeemStatus(Id, status);
            }
            return Json(new
            {
                message = sMessage
            });
        }

        [Route("redeems")]
        public ActionResult redeem()
        {
            if (Session["UserID"] != null)
            {
                return View("RedemptionGift");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public JsonResult RedemptionGifts(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchRedemptionGifts(model);

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
        public DataTableResult<CustomRedemptionGift> SearchRedemptionGifts(DataTableAjaxPostModel model)
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
            TittleRedeemServices _Service = new TittleRedeemServices();
            DataTableResult<CustomRedemptionGift> result = new Models.DataTableResult<CustomRedemptionGift>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            result.result = _Service.GetRedemptionGifts(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomRedemptionGift>();
            }
            return result;
        }

        [HttpGet]
        public JsonResult IsRedemptionGiftExist(string name, long id)
        {
            TittleRedeemServices service = new TittleRedeemServices();
            bool isExist = false;
            redeem data = service.GetRedemptionGiftByKey(name);
            if (data == null)
                isExist = false;
            else if (data.id == id)
                isExist = false;
            else
                isExist = true;

            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(CustomRedemptionGift model)
        {
            string sMessage = "";
            if (ModelState.IsValid)
            {
                TittleRedeemServices service = new TittleRedeemServices();
                long nID = 0;
                sMessage = service.SaveOrUpdateRedemptionGift(model, ref nID);
            }

            return Json(new
            {
                message = sMessage
            });
        }

        public ActionResult AddRedemptionGift(int Id)
        {
            TittleRedeemServices service = new TittleRedeemServices();

            CustomRedemptionGift model = new CustomRedemptionGift();

            if (Id > 0)
            {
                redeem _data = service.GetRedemptionGiftInfo(Id);
                model.type = _data.type;
                model.name = _data.name;
                model.points = _data.points;
                model.frequency = _data.frequency;
                model.id = _data.id;
            }
            return PartialView("AddRedemptionGift", model);
        }

        [HttpPost]
        public ActionResult DeleteRedemptionGift(int Id)
        {
            TittleRedeemServices service = new TittleRedeemServices();
            string sMessage = "";
            if (Id > 0)
            {
                service.DeleteRedemptionGift(Id, ref sMessage);
            }
            return Json(new
            {
                message = sMessage
            });
        }
    }
}