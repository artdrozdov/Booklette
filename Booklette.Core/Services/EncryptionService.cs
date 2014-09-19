using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Booklette.Core.Services
{
    public class EncryptionService
    {
        private static EncryptionService _Instance;

        private static object Locker = new object(); 

         public static EncryptionService Instance
         {
             get
             {
                 if (_Instance == null)
                 {
                     lock (Locker)
                     {
                         if (_Instance == null)
                         {
                             _Instance = new EncryptionService();
                         }
                     }
                 }
                 return _Instance;
             }
         }
        private EncryptionService() {
            Hasher = MD5.Create();
        }

        private MD5 Hasher { get; set; }

        public string EncryptMD5(string data) {

            var arr =  Hasher.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder result = new StringBuilder();
            foreach (var Byte in arr)
            {
                result.Append(Byte.ToString("X2"));
            }
            return result.ToString();
        }
    }
}
