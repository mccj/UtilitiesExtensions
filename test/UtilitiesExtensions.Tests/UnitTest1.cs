using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace UtilitiesExtensionsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
          var i=  "0001".AsInt();
            Assert.AreEqual(i, 1);
        }
    }
}
