using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booklette.Core.Entities;
using System.Text.RegularExpressions;
using Booklette.Core.Services;

namespace Booklette.Core.BookParser
{
    public class PageCreator
    {
        public const int StrLen = 81;
        public const int Rows = 32;
        public List<Page> Pages = new List<Page>();
        private int _CurrentLine { get; set; }
        private int LastPageNo = 0;
        private Page Tmp = new Page();
        private Regex Regex { get; set; }

        public PageCreator() {
            Regex = new Regex(@"</?\S*>");
        }

        public void AddPage(string tagName, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                string str = Regex.Replace(value, String.Empty);
                int charlength = str.Length;
                int rowsNeed = (int)Math.Ceiling((double)charlength / StrLen);
                if (_CurrentLine + rowsNeed <= Rows)
                {
                    Tmp.Content += "<" + tagName + ">" + value + "</" + tagName + ">";
                    _CurrentLine += rowsNeed;
                }
                else
                {
                    int rest = Rows - _CurrentLine;
                    Tmp.Content += value.Substring(0, rest * StrLen) + "</" + tagName + ">";
                    Tmp.Order = ++LastPageNo;
                    Pages.Add(Tmp);
                    Tmp = new Page();
                    _CurrentLine = 0;
                    AddPage(tagName, value.Substring(rest * StrLen));
                }
            }
        }
    }
}
