using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Model.DTO;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Index")]
        public ActionResult UserLogin(CustomUserLogin _User)
        {
            if (ModelState.IsValid)
            {
                TittleUserServices _UserService = new TittleUserServices();
                string strRet = "";
                long nId = 0;
                _UserService.ValidateLoginInfo(_User.Email, _User.Password, ref nId, ref strRet);
                if (string.IsNullOrEmpty(strRet))
                {
                    Session["UserID"] = nId;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.ErrorMessage = strRet;
                    return View("Index", _User);
                }
            }
            ViewBag.ErrorMessage = "Please enter Email/Password.";
            return View("Index");
        }

        [HttpPost]
        public ActionResult UserLogout()
        {
            Session["UserID"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}