using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Booklette.Core.Services;
using ServiceStack.Text;
using System.Web.Security;
using Booklette.Core.Entities;
using Booklette.Models;


namespace Booklette.Controllers
{
    [Authorize]
    public class ReaderController : Controller
    {
        // GET: Reader
        [HttpGet]
        public ActionResult Book(long? id)
        {
            var model = new ReaderModel();
            if (id != null)
            {
                var ticket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                var user = JsonSerializer.DeserializeFromString<User>(ticket.UserData);
                model.Page = ReaderService.Instance.GetLastPage(user.Id,(long)id);
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/Reader";
                return PartialView("Book", model);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult NextPage(long? id)
        {
            var model = new ReaderModel();
            if (id != null)
            {
                var ticket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                var user = JsonSerializer.DeserializeFromString<User>(ticket.UserData);
                model.Page = ReaderService.Instance.GetNextPage(user.Id, (long)id);
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/Reader/Book/"+id;
                return PartialView("Book", model);
            }
            return View("Book",model);
        }
        [HttpGet]
        public ActionResult PrevPage(long? id)
        {
            var model = new ReaderModel();
            if (id != null)
            {
                var ticket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                var user = JsonSerializer.DeserializeFromString<User>(ticket.UserData);
                model.Page = ReaderService.Instance.GetPrevPage(user.Id, (long)id);
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/Reader/Book/"+id;
                return PartialView("Book", model);
            }
            return View("Book", model);
        }
    }
}