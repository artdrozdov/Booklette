using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booklette.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        // GET: Store
        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/Store";
                return PartialView("Index");
            }
            return View();
        }
    }
}