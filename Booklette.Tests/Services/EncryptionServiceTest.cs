using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Booklette.Core.Services;

namespace Booklette.Tests.Services
{
    [TestClass]
    public class EncryptionServiceTest
    {
        [TestMethod]
        public void Encryption_Service_MD5()
        {
            string foo = EncryptionService.Instance.EncryptMD5("Foo");
            string bar = EncryptionService.Instance.EncryptMD5("Bar");
            Assert.AreNotEqual(foo, bar);
            string foo2 = EncryptionService.Instance.EncryptMD5("Foo");
            Assert.AreEqual(foo,foo2);
            string bar2 = EncryptionService.Instance.EncryptMD5("Bar");
            Assert.AreEqual(bar, bar2);
        }
    }
}
