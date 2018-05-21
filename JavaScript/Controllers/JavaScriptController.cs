using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace JavaScript.Controllers
{
    public class JavaScriptController : Controller
    {
        // GET: JavaScript
        public ActionResult Index()
        {
            ViewBag.userID = User.Identity.GetUserId();

            return View();
        }
        [Authorize]
        public ActionResult Create()
        {

            ViewBag.userID = User.Identity.GetUserId();

            return View();

        }
    }
}