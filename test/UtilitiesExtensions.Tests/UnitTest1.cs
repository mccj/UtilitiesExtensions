using Xunit;
using System.Linq;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            new string[] { "" }.OrderBy("");
        }
    }
}
