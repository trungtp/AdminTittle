using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TittleAdmin.Model.DTO;
using TittleAdmin.Model.Model;
using TittleAdmin.Models;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class PromoCodeController : Controller
    {
        // GET: PromoCode
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
        public JsonResult PromoCodes(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchPromoCodes(model);

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
        public DataTableResult<CustomPromoCode> SearchPromoCodes(DataTableAjaxPostModel model)
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
            TittlePromoCodeServices _PromoCodeService = new TittlePromoCodeServices();
            DataTableResult<CustomPromoCode> result = new Models.DataTableResult<CustomPromoCode>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            result.result = _PromoCodeService.GetPromoCodesList(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomPromoCode>();
            }
            return result;
        }
        
        [HttpGet]
        public JsonResult IsPromoCodeExist(string CodeID, long id)
        {
            TittlePromoCodeServices promoCodeService = new TittlePromoCodeServices();
            bool isExist = false;
            promo_codes promoCode = promoCodeService.GetPromoCodeInfoByCode(CodeID);
            if (promoCode == null)
                isExist = false;
            else if (promoCode.id == id)
                isExist = false;
            else
                isExist = true;

            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(CustomPromoCode model)
        {
            string sMessage = "";
            if (ModelState.IsValid)
            {
                TittlePromoCodeServices promoCodeService = new TittlePromoCodeServices();
                long nID = 0;
                if (model.Quantity == "Unlimited")
                {
                    model.Quantity = "-1";
                }
                if (model.promoValueType == PromoValueType.FlatRate)
                {
                    model.type = "flat_rate";
                }
                else
                {
                    model.type = "percentage";
                }
                model.TypeValue = EnumExtended.GetEnumDisplayName<PromoTypes>(model.promoType).ToLower();

                sMessage = promoCodeService.SaveOrUpdatePromoCode(model, ref nID);
            }

            return Json(new
            {
                message = sMessage
            });
        }

        public ActionResult AddPromoCode(int Id)
        {
            TittlePromoCodeServices promoCodeService = new TittlePromoCodeServices();

            CustomPromoCode model = new CustomPromoCode();

            if (Id > 0)
            {
                promo_codes promoCode = promoCodeService.GetPromoCodeInfo(Id);
                model.CodeID = promoCode.code;
                model.Description = promoCode.description;
                model.EndDate = promoCode.end_date.ToString("dd/MM/yyyy");
                model.StartDate = promoCode.start_date.ToString("dd/MM/yyyy");
                model.id = promoCode.id;
                model.Quantity = promoCode.quantity.ToString();
                model.Rules = promoCode.rule;
                model.type = promoCode.type;
                model.Value = promoCode.value.ToString();
                if (promoCode.type == "percentage")
                {
                    model.promoValueType = PromoValueType.Percentage;
                }
                else
                {
                    model.promoValueType = PromoValueType.FlatRate;
                }
                switch (promoCode.promo_type)
                {
                    case "event":
                        model.promoType = PromoTypes.Event;
                        break;
                    case "internal":
                        model.promoType = PromoTypes.Internal;
                        break;
                    case "offer":
                        model.promoType = PromoTypes.Offer;
                        break;
                    case "test":
                        model.promoType = PromoTypes.Test;
                        break;
                    case "partner":
                        model.promoType = PromoTypes.Partner;
                        break;
                    default:
                        model.promoType = PromoTypes.Event;
                        break;
                }
            }
            else
            {
                model.promoValueType = PromoValueType.FlatRate;
                model.promoType = PromoTypes.Event;
            }
            if (model.Quantity == "-1")
            {
                model.Quantity = "Unlimited";
            }
            return PartialView("AddPromoCode", model);
        }

        [HttpPost]
        public ActionResult DeletePromoCode(int Id)
        {
            TittlePromoCodeServices promoCodeService = new TittlePromoCodeServices();
            string sMessage = "";
            if (Id > 0)
            {
                promoCodeService.DeletePromoCode(Id, ref sMessage);
            }
            return Json(new
            {
                message = sMessage
            });
        }
        
    }
}