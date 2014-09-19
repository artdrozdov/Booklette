using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Booklette.Core.Entities;
using Booklette.Core.Services;
using System.Data;
using ServiceStack.OrmLite;
using System.Text.RegularExpressions;
using System.IO;

namespace Booklette.Core.BookParser
{
    public class Fb2Parser
    {
        public PageCreator Creator { get; set; }

        public long BID { get; set; }

        public void Parse(string filePath) {
            XDocument doc = XDocument.Load(filePath);
            Book book = new Book();
            Creator = new PageCreator();
            string pattern = " xmlns=" + '"' + ".*" + '"';
            Regex rgx = new Regex(pattern);
            
            foreach(XElement x in doc.Root.Elements()) {
                if (x.Name.LocalName.ToLower() == "description") {
                    foreach (XElement y in x.Elements())
                    {
                        if (y.Name.LocalName.ToLower() == "title-info") {
                            foreach (var el in y.Elements()) {

                                switch(el.Name.LocalName.ToLower()){
                                    case "lang":{
                                        book.Localization = el.Value;
                                    } break;
                                    case "book-title":{
                                        book.Title = el.Value;
                                    } break;
                                    case "author": { 
                                        foreach(var author in el.Elements()){
                                            switch(author.Name.LocalName.ToLower()){
                                                case "first-name": {
                                                    book.Author += author.Value;
                                                } break;
                                                case "last-name": {
                                                    book.Author += " " + author.Value;
                                                } break;
                                            }
                                        }
                                    } break;
                                }
                            }
                        }
                    }
                }
                if (x.Name.LocalName.ToLower() == "body")
                {
                    foreach (var section in x.Elements())
                    {
                        if (section.HasElements)
                        {

                            foreach (var line in section.Elements())
                            {
                                Creator.AddPage(line.Name.LocalName, line.Value);
                            }
                            
                            var reader = section.CreateReader();
                            reader.MoveToContent();
                            Creator.AddPage(section.Name.LocalName,reader.ReadInnerXml());
                            
                        }
                        else
                        {
                            Creator.AddPage(section.Name.LocalName, section.Value);
                        }
                    }
                }
            }
            using (var db = StorageService.Instance.DbFactory.OpenDbConnection()) {
                db.Insert<Book>(book);
                BID = db.GetLastInsertId();
                this.Creator.Pages.ForEach(x => x.BID = BID);
                db.InsertAll<Page>(this.Creator.Pages);
            }
            File.Delete(filePath);
        }
    }
}
