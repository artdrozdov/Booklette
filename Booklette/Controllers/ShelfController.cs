using System.Web.Mvc;
using Booklette.Models;
using Booklette.Core.Services;
using ServiceStack.OrmLite;
using Booklette.Core.Entities;

namespace Booklette.Controllers
{
    [Authorize]
    public class ShelfController : Controller
    {
        //
        // GET: /Shelf/
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ShelfModel();
            using (var db = StorageService.Instance.DbFactory.OpenDbConnection())
            {
                model.Books = db.Select<Book>();
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/Shelf";
                return PartialView("Index", model);
            }
            return View(model);
        }

    }
}
