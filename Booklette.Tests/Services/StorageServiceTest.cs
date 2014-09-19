using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Booklette.Core.Services;
using ServiceStack.DataAnnotations;
using System.Linq;
using System.Data;
using ServiceStack.OrmLite;

namespace Booklette.Tests.Services
{
    [TestClass]
    public class StorageServiceTest
    {
        [TestMethod]
        public void DB_Connection_Test()
        {
            try{
                var db = StorageService.Instance;
            }
            catch(Exception e){
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void DB_CRUD_Test() {

            try
            {
                var db = StorageService.Instance.DbFactory.OpenDbConnection();
                using (var tr = db.OpenTransaction())
                {
                    try
                    {
                        db.CreateTable<TestData>();
                        var testData = new TestData { Name = "Test", Date = DateTime.Now };
                        db.Insert<TestData>(testData);
                        var test = db.SingleWhere<TestData>("Name", "Test");
                        Assert.AreEqual(testData.Name, test.Name);
                        Assert.AreEqual(testData.Date.Millisecond, test.Date.Millisecond);
                        db.DropTable<TestData>();
                    }
                    catch (Exception e)
                    {
                        Assert.Fail(e.Message);
                    }
                    finally
                    {
                        tr.Rollback();
                    }

                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }

    public class TestData {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
