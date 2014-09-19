using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using ServiceStack.DataAnnotations;

namespace Booklette.Core.Entities
{
    [Alias("Books")]
    public class Book
    {
        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Localization { get; set; }

    }
}
