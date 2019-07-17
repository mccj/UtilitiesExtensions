using System
/* 项目“UtilitiesExtensions.Tests (NET46)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/

/* 项目“UtilitiesExtensions.Tests (NET45)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/

/* 项目“UtilitiesExtensions.Tests (NET48)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/

/* 项目“UtilitiesExtensions.Tests (NET452)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/

/* 项目“UtilitiesExtensions.Tests (NET451)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/

/* 项目“UtilitiesExtensions.Tests (NET471)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/

/* 项目“UtilitiesExtensions.Tests (NET472)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/

/* 项目“UtilitiesExtensions.Tests (NET461)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/

/* 项目“UtilitiesExtensions.Tests (NET462)”的未合并的更改
在此之前:
using System.Text;
在此之后:
using System.Globalization;
*/
.Linq;
using System.Text;
using Xunit;

namespace UtilitiesExtensionsTest
{
    public class StringExtTest
    {
        [Fact]
        public void StringExtensionsTest()
        {
            var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            numFormat.CurrencySymbol = "ルピー";
            numFormat.CurrencyGroupSeparator = "`";
            numFormat.CurrencyGroupSizes = new[] { 4 };

            Assert.Equal(12345, "12345".ToInt32());
            //Assert.Equal(12345, @"12,345\".ToInt32(NumberStyles.Currency));
            Assert.Equal(12345, "12345".ToInt32(numFormat));
            //Assert.Equal(123456789, "1`2345`6789ルピー".ToInt32(NumberStyles.Currency, numFormat));

            Assert.Equal(999, "999".ToInt32OrNull());
            //Assert.Equal(999999, @"999,999\".ToInt32OrNull(NumberStyles.Currency));
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
}