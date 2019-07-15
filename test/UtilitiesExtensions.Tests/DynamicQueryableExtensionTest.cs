using System;
using System.Text;
using System.Globalization;
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
        public void OrderByTest()
        {
            var s1 = DynamicQueryableExtension.OrderBy(tms, "Age").FirstOrDefault();
            var s2 = DynamicQueryableExtension.OrderBy(tms, "Age desc").FirstOrDefault();
            Assert.Equal(5, s1.Age);
            Assert.Equal(6, s2.Age);

            //var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            //numFormat.CurrencySymbol = "ルピー";
            //numFormat.CurrencyGroupSeparator = "`";
            //numFormat.CurrencyGroupSizes = new[] { 4 };

            //Assert.Equal(12345, "12345".ToInt32());
            ////Assert.Equal(12345, @"12,345\".ToInt32(NumberStyles.Currency));
            //Assert.Equal(12345, "12345".ToInt32(numFormat));
            ////Assert.Equal(123456789, "1`2345`6789ルピー".ToInt32(NumberStyles.Currency, numFormat));

            //Assert.Equal(999, "999".ToInt32OrNull());
            ////Assert.Equal(999999, @"999,999\".ToInt32OrNull(NumberStyles.Currency));
            //Assert.Equal(999999999, "9`9999`9999ルピー".ToInt32OrNull(NumberStyles.Currency, numFormat));

            //Assert.Equal(default(int), "xxxx".ToInt32OrDefault());
            //Assert.Equal(default(int), "xxxx".ToInt32OrDefault(NumberStyles.Currency));
            //Assert.Equal(default(int), "xxxx".ToInt32OrDefault(NumberStyles.Currency, numFormat));

            //Assert.Equal(int.MinValue, "xxxx".ToInt32OrDefault(int.MinValue));
            //Assert.Equal(-1, "xxxx".ToInt32OrDefault(NumberStyles.Currency, -1));
            //Assert.Equal(int.MaxValue, "xxxx".ToInt32OrDefault(NumberStyles.Currency, numFormat, int.MaxValue));

            //Assert.Null("!00".ToInt32OrNull());
            //Assert.Null(@"QQQ,999\".ToInt32OrNull(NumberStyles.Currency));
            //Assert.Null("9!9999!9999ルピー".ToInt32OrNull(NumberStyles.Currency, numFormat));
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