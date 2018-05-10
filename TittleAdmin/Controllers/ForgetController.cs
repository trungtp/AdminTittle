using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class ForgetController : Controller
    {
        // GET: Forget
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string Email)
        {
            if (!string.IsNullOrEmpty(Email.Trim()))
            {
                TittleUserServices _UserService = new TittleUserServices();
                string strRet = "";
                long nId = 0;
                _UserService.ValidateForgotEmail(Email, ref strRet);
                if (string.IsNullOrEmpty(strRet))
                {
                    Session["UserID"] = nId;
                    return View("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = strRet;
                    return View("Index");
                }
            }
            ViewBag.ErrorMessage = "Please enter your email.";
            return View("Index");
        }
    }
}