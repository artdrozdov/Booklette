using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace Booklette.Core.Entities
{
    [Alias("Pages")]
    public class Page
    {
        public Page()
        {
            Id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(Book))]
        public long BID { get; set; }

        public int Order { get; set; }

        public string Content { get; set; }

    }
}
