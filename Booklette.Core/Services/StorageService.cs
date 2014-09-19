using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using System.Data;
using Booklette.Core.Entities;

namespace Booklette.Core.Services
{
    public class StorageService
    {

        private static StorageService _Instance { get; set; }

        private static object Locker = new object();

        public static StorageService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (Locker)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new StorageService();
                        }
                    }
                }
                return _Instance;
            }
        }
        private StorageService() {
            ConnectionString = "Server=127.0.0.1;Port=5432;User Id=postgres; Password=1337; Database=Booklette.Main;";
            DbFactory = new OrmLiteConnectionFactory(ConnectionString, PostgreSqlDialect.Provider);
            InitStorage();
        }

        public OrmLiteConnectionFactory DbFactory { get; set; }

        private string ConnectionString { get; set; }

        private void InitStorage() {
            using (var db = DbFactory.OpenDbConnection()) {
                try
                {
                    db.CreateTableIfNotExists<User>();
                    db.CreateTableIfNotExists<Book>();
                    db.CreateTableIfNotExists<Page>();
                    db.CreateTableIfNotExists<UserBooks>();

                }
                catch (Exception e) { 
                    //Log error
                }
            }
        }

    }
}
