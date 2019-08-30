using System.Linq;
using Xunit;

namespace UtilitiesExtensionsTest
{
    public class DynamicQueryableExtensionTest
    {
        private readonly Tm[] tms = new[] { new Tm { Name = "a", Age = 5 }, new Tm { Name = "b", Age = 6 } };
        public DynamicQueryableExtensionTest()
        {

        }
        [Fact]
        public void ToHashSetTest()
        {
            var r = tms.ToHashSet();
            Assert.Equal("HashSet`1", r.GetType().Name);
        }

        //[TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        //public void ToInt32Test_2()
        //{
        //    "XXXXX".ToInt32();
        //}
        //[TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        //public void ToInt32Test_3()
        //{
        //    @"XXXXX\".ToInt32(NumberStyles.Currency);
        //}
        //[TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        //public void ToInt32Test_4()
        //{
        //    "ABCDE".ToInt32(CultureInfo.CurrentCulture.NumberFormat);
        //}
        //[TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        //public void ToInt32Test_5()
        //{
        //    var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        //    numFormat.CurrencySymbol = "ルピー";
        //    numFormat.CurrencyGroupSeparator = "`";
        //    numFormat.CurrencyGroupSizes = new[] { 4 };
        //    "123,456,789G".ToInt32(NumberStyles.Currency, numFormat);
        //}
    }

    public class Tm
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}