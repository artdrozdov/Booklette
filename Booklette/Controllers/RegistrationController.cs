using System.Web.Mvc;
using Booklette.Models;
using Booklette.Core.Entities;
using Booklette.Core.Services;
using ServiceStack.OrmLite;

namespace Booklette.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /Registration/
        [HttpGet]
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Shelf");
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/Registration";
                return PartialView("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegistrationModel model)
        {
            if (ModelState.IsValid && model.Password == model.RepeatPasword) {
                var user = new User();
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.SecurityKey = EncryptionService.Instance.EncryptMD5(model.Password+model.Email);
                using (var db = StorageService.Instance.DbFactory.OpenDbConnection()) {
                    if (db.FirstOrDefault<User>(x => x.Email == model.Email) == null)
                    {
                        db.Insert<User>(user);
                        return RedirectToAction("Index", "Login");
                    }
                    else {
                        ModelState.AddModelError("Email","Email already registered");
                    }
                }
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                //Response.Headers["X-PJAX-URL"] = "/Registration";
                return PartialView("Index");
            }
            return View();
        }

    }
}
