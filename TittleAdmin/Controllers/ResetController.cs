using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Model.DTO;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class ResetController : Controller
    {
        // GET: Reset
        public ActionResult Index()
        {
            string qstr = Convert.ToString(Request.QueryString["key"]);
            if (!string.IsNullOrEmpty(qstr))
            {
                TittleUserServices _UserService = new TittleUserServices();
                string sMessage = "";
                CustomPasswordReset _user = _UserService.ValidateResetKey(qstr, ref sMessage);
                if (sMessage != "")
                {
                    ViewBag.ErrorMessage = sMessage;
                    return View();
                }
                else
                {
                    return View(_user);
                }
            }
            ViewBag.ErrorMessage = "No token found.";
            return View();
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(CustomPasswordReset reset)
        {
            if (ModelState.IsValid)
            {
                TittleUserServices _UserService = new TittleUserServices();
                if (_UserService.ResetPassword(reset.UserID, reset.Password, reset.Token))
                {
                    ViewBag.ErrorMessage = "Password reset successfully.";
                    return RedirectToAction("Index", "Login");
                }
            }
            ViewBag.ErrorMessage = "Please fill all the fields.";
            return View("Index", reset);
        }
    }
}