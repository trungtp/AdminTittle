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
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                TittleLanguageServices _LangService = new TittleLanguageServices();
                List<CustomLanguage> langs = _LangService.GetLanguagesList();
                return View(langs);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult LanguageDropdown()
        {
            TittleLanguageServices _LangService = new TittleLanguageServices();
            List<CustomLanguage> langs = _LangService.GetLanguagesList();
            return PartialView("LanguageDropdown", langs);
        }

        [HttpPost]
        public JsonResult LanguageTranslations(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            var result = SearchLanguageTranslations(model);

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
        public DataTableResult<CustomLanguageTranslation> SearchLanguageTranslations(DataTableAjaxPostModel model)
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
            TittleLanguageServices _LangService = new TittleLanguageServices();
            DataTableResult<CustomLanguageTranslation> result = new Models.DataTableResult<CustomLanguageTranslation>();
            int filteredResultsCount = 0;
            int totalResultsCount = 0;
            result.result = _LangService.GetTranslationsList(searchBy, take, skip, sortBy, sortDir, model.CustomData, out filteredResultsCount, out totalResultsCount);
            result.filteredResultsCount = filteredResultsCount;
            result.totalResultsCount = totalResultsCount;
            if (result.result == null)
            {
                // empty collection...
                return new DataTableResult<CustomLanguageTranslation>();
            }
            return result;
        }

        public ActionResult CreateNewLanguage()
        {
            TittleLanguageServices _LangService = new TittleLanguageServices();
            List<CustomLanguage> langs = _LangService.GetLanguagesList();
            CustomNewLanguage model = new CustomNewLanguage();
            model.AddedLanguages = langs;
            model.AvailableLanguages = new Dictionary<string, string>();
            foreach (var item in CoreKeyValue.AvailableLanguages)
            {
                bool isFound = false;
                foreach (var lang in langs)
                {
                    if (item.Value == lang.LangLabel)
                    {
                        isFound = true;
                        break;
                    }
                }
                if (!isFound)
                    model.AvailableLanguages.Add(item.Key, item.Value);
            }
            return PartialView("CreateNewLanguage", model);
        }
        public ActionResult CreateNewKey()
        {
            return PartialView("CreateNewKey");
        }

        [HttpPost]
        public ActionResult CreateNewLanguage(CustomNewLanguage model)
        {
            string sMessage = "";
            if (ModelState.IsValid)
            {
                TittleLanguageServices languageService = new TittleLanguageServices();
                long nID = 0;

                sMessage = languageService.SaveOrUpdateLanguage(model, ref nID);
            }

            return Json(new
            {
                message = sMessage
            });
        }

        [HttpPost]
        public ActionResult DeleteLanguage(long Id)
        {
            TittleLanguageServices langServices = new TittleLanguageServices();
            string sMessage = "";
            if (Id > 0)
            {
                sMessage = langServices.DeleteLanguage(Id);
            }
            return Json(new
            {
                message = sMessage
            });
        }

        [HttpPost]
        public ActionResult CreateNewKey(key model)
        {
            string sMessage = "";
            if (ModelState.IsValid)
            {
                TittleLanguageServices languageService = new TittleLanguageServices();
                long nID = 0;

                sMessage = languageService.SaveOrUpdateKey(model, ref nID);
            }

            return Json(new
            {
                message = sMessage
            });
        }

        [HttpPost]
        public ActionResult DeleteKey(long Id)
        {
            TittleLanguageServices langServices = new TittleLanguageServices();
            string sMessage = "";
            if (Id > 0)
            {
                sMessage = langServices.Deletekey(Id);
            }
            return Json(new
            {
                message = sMessage
            });
        }

        [HttpPost]
        public ActionResult UpdateKey(long Id, string value)
        {
            TittleLanguageServices langServices = new TittleLanguageServices();
            string sMessage = "";
            bool status = false;
            if (Id > 0)
            {
                sMessage = langServices.Updatekey(Id, value);
                if (sMessage == "Success")
                {
                    status = true;
                    sMessage = "Key updated successfully";
                }
            }
            return Json(new
            {
                status = status,
                message = sMessage
            });
        }

        [HttpPost]
        public ActionResult UpdateTranslation(long Id, string value)
        {
            TittleLanguageServices langServices = new TittleLanguageServices();
            string sMessage = "";
            bool status = false;
            if (Id > 0)
            {
                sMessage = langServices.UpdateTranslation(Id, value);
                if (sMessage == "Success")
                {
                    status = true;
                    sMessage = "Translation updated successfully";
                }
            }
            return Json(new
            {
                status = status,
                message = sMessage
            });
        }
    }
}