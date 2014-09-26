using System.Linq;
using System.Web;
using System.Web.Mvc;
using Booklette.Models;
using Booklette.Core.BookParser;
using System.Web.Security;
using ServiceStack.Text;
using Booklette.Core.Entities;
using Booklette.Core.Services;
using ServiceStack.OrmLite;

namespace Booklette.Controllers
{
    [Authorize]
    public class ConsoleController : Controller
    {
        // GET: Console
        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/Console";
                return PartialView("Index", new ConsoleModel());
            }
            return View(new ConsoleModel());
        }

        [HttpPost]
        public ActionResult UploadBook(HttpPostedFileBase Book) {
            string filePath = HttpRuntime.AppDomainAppPath + "/Books/" + Book.FileName;
            Book.SaveAs(filePath);
            var parser = new Fb2Parser();
            parser.Parse(filePath);
            var ticket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
            var user = JsonSerializer.DeserializeFromString<User>(ticket.UserData);
            var userBook = new UserBooks();
            userBook.BID = parser.BID;
            userBook.UID = user.Id;
            var page = parser.Creator.Pages.FirstOrDefault(x => x.Order == 1);
            userBook.LastPage = page.Id;
            userBook.PageNumber = page.Order;
            using (var db = StorageService.Instance.DbFactory.OpenDbConnection())
            {
                db.Insert<UserBooks>(userBook);
            }
            if (Request.Headers["X-PJAX"] == "true")
            {
                Response.Headers["X-PJAX-URL"] = "/Console";
                return PartialView("Index", new ConsoleModel());
            }
            return View("Index", new ConsoleModel());
        }
    }
}