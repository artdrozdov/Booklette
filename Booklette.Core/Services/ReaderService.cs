using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booklette.Core.Entities;
using Booklette.Core.Services;
using ServiceStack.OrmLite;

namespace Booklette.Core.Services
{
    public class ReaderService
    {
        private static ReaderService _instance;

        public static ReaderService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReaderService();
                }
                return _instance;
            }
        }

        public Page GetLastPage(long UID, long BID)
        {
            using (var db = StorageService.Instance.DbFactory.OpenDbConnection())
            {
                var userBook = db.FirstOrDefault<UserBooks>(x => x.BID == BID && x.UID == UID);
                if (userBook != null)
                {
                    return db.FirstOrDefault<Page>(x => x.Id == userBook.LastPage);
                }
            }
            return null;
        }

        public Page GetNextPage(long UID, long BID)
        {
            using (var db = StorageService.Instance.DbFactory.OpenDbConnection())
            {
                var userBook = db.FirstOrDefault<UserBooks>(x => x.BID == BID && x.UID == UID);
                if (userBook != null)
                {
                    var nextPage = db.FirstOrDefault<Page>(x => x.BID == BID && x.Order == userBook.PageNumber+1);
                    db.UpdateOnly(new UserBooks { LastPage = nextPage.Id, PageNumber = nextPage.Order }, x => new { x.PageNumber, x.LastPage }, x => x.BID == BID && x.UID == UID);
                    return nextPage;
                }
            }
            return null;
        }

        public Page GetPrevPage(long UID, long BID)
        {
            using (var db = StorageService.Instance.DbFactory.OpenDbConnection())
            {
                var userBook = db.FirstOrDefault<UserBooks>(x => x.BID == BID && x.UID == UID);
                if (userBook != null)
                {
                    var prevPage = db.FirstOrDefault<Page>(x => x.BID == BID && x.Order == userBook.PageNumber - 1);
                    db.UpdateOnly(new UserBooks { LastPage = prevPage.Id, PageNumber = prevPage.Order }, x => new { x.PageNumber, x.LastPage }, x => x.BID == BID && x.UID == UID);
                    return prevPage;
                }
            }
            return null;
        }
    }
}
