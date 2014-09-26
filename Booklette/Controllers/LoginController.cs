using System;
using System.Web;
using System.Web.Mvc;
using Booklette.Models;
using System.Web.Security;
using Booklette.Core.Entities;
using Booklette.Core.Services;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using System.Linq;
using System.Data;
using ServiceStack.DataAnnotations;

namespace Booklette.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Shelf");
            }
            if (Request.Headers["X-PJAX"] == "true") {
                Response.Headers["X-PJAX-URL"] = "/";
                return PartialView("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model) {
            
            if(ModelState.IsValid){
                string hash = EncryptionService.Instance.EncryptMD5(model.Password+model.Email);
                using (var db = StorageService.Instance.DbFactory.OpenDbConnection())
                {
                    var user = db.First<User>(x => x.Email == model.Email && x.SecurityKey == hash);
                    if (user != null)
                    {
                        string userData = JsonSerializer.SerializeToString<User>(user);
                        
                        var ticket = new FormsAuthenticationTicket(1,"UserCredentials",DateTime.Now,DateTime.Now.Add(FormsAuthentication.Timeout),true,userData);
                        //FormsAuthentication.RedirectFromLoginPage(model.Email, true);
                        var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                        {
                            Value = encryptedTicket,
                            Expires = ticket.Expiration
                        };
                        HttpContext.Response.Cookies.Set(cookie);
                        return RedirectToAction("Index", "Shelf");
                    }
                }
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                //Response.Headers["X-PJAX-URL"] = "/";
                return PartialView("Index");
            }
            return View();
        }

        public ActionResult Logout() {
            if (HttpContext.User.Identity.IsAuthenticated) {
                FormsAuthentication.SignOut();
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/";
                return PartialView("Index");
            }
            return View("Index");
        }

    }
}
