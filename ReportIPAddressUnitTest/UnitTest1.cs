using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportIPAddress;

namespace ReportIPAddressUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetIPAddressTest()
        {
            var ipArray =new IPAddressService().GetIPAddress();

            Assert.IsFalse(string.IsNullOrEmpty(ipArray));
        }
    }
}
