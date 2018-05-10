using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Model;
using TittleAdmin.Service.Implementations;

namespace TittleAdmin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Default
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
    }
}