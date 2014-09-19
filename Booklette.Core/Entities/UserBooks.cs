using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;


namespace Booklette.Core.Entities
{
    public class UserBooks
    {
        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [ForeignKey(typeof(User))]   
        public long UID { get; set; }

        [ForeignKey(typeof(Book))]
        public long BID { get; set; }

        public Guid? LastPage { get; set; }

        public int PageNumber { get; set; }
    }
}
