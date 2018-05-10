using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TittleAdmin.Models;

namespace TittleAdmin.Controllers
{
    public class MessageController : Controller
    {
        [HttpPost]
        public ActionResult Show(MessageInfo message)
        {
            return PartialView("Show", message);
        }
    }
}