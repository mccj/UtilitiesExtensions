using System;
using System.Text;
using System.Globalization;
using System.Linq;
using Xunit;

namespace UtilitiesExtensionsTest
{
    public class StringExtTest
    {
        [Fact]
        public void StringExtensionsTest()
        {
            var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            numFormat.CurrencySymbol = "¥ë¥Ô©`";
            numFormat.CurrencyGroupSeparator = "`";
            numFormat.CurrencyGroupSizes = new[] { 4 };

            Assert.Equal(12345, "12345".ToInt32());
            //Assert.Equal(12345, @"12,345\".ToInt32(NumberStyles.Currency));
            Assert.Equal(12345, "12345".ToInt32(numFormat));
            //Assert.Equal(123456789, "1`2345`6789¥ë¥Ô©`".ToInt32(NumberStyles.Currency, numFormat));

            Assert.Equal(999, "999".ToInt32OrNull());
            //Assert.Equal(999999, @"999,999\".ToInt32OrNull(NumberStyles.Currency));
            //Assert.Equal(999999999, "9`9999`9999¥ë¥Ô©`".ToInt32OrNull(NumberStyles.Currency, numFormat));

            //Assert.Equal(default(int), "xxxx".ToInt32OrDefault());
            //Assert.Equal(default(int), "xxxx".ToInt32OrDefault(NumberStyles.Currency));
            //Assert.Equal(default(int), "xxxx".ToInt32OrDefault(NumberStyles.Currency, numFormat));

            //Assert.Equal(int.MinValue, "xxxx".ToInt32OrDefault(int.MinValue));
            //Assert.Equal(-1, "xxxx".ToInt32OrDefault(NumberStyles.Currency, -1));
            //Assert.Equal(int.MaxValue, "xxxx".ToInt32OrDefault(NumberStyles.Currency, numFormat, int.MaxValue));

            //Assert.Null("!00".ToInt32OrNull());
            //Assert.Null(@"QQQ,999\".ToInt32OrNull(NumberStyles.Currency));
            //Assert.Null("9!9999!9999¥ë¥Ô©`".ToInt32OrNull(NumberStyles.Currency, numFormat));
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
        //    numFormat.CurrencySymbol = "¥ë¥Ô©`";
        //    numFormat.CurrencyGroupSeparator = "`";
        //    numFormat.CurrencyGroupSizes = new[] { 4 };
        //    "123,456,789G".ToInt32(NumberStyles.Currency, numFormat);
        //}
    }
}