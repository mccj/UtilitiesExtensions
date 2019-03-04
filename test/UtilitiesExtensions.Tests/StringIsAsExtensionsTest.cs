using System.Linq;
using Xunit;

namespace UtilitiesExtensionsTest
{
    public class StringIsAsExtensionsTest
    {
        [Fact]
        public void StringAsTest()
        {
            Assert.Equal(12345, "12345".AsInt32());
            //Assert.Equal(12345, @"12,345\".AsInt32());
            Assert.Equal(12345, "12345".AsInt32());
            //Assert.Equal(123456789, "1`2345`6789¥ë¥Ô©`".AsInt32());

            //Assert.Equal(999, "999".AsInt32OrNull());
            //Assert.Equal(999999, @"999,999\".AsInt32OrNull(NumberStyles.Currency));
            //Assert.Equal(999999999, "9`9999`9999¥ë¥Ô©`".AsInt32OrNull(NumberStyles.Currency, numFormat));

            //Assert.Equal(default(int), "xxxx".AsInt32OrDefault());
            //Assert.Equal(default(int), "xxxx".AsInt32OrDefault(NumberStyles.Currency));
            //Assert.Equal(default(int), "xxxx".AsInt32OrDefault(NumberStyles.Currency, numFormat));

            //Assert.Equal(int.MinValue, "xxxx".AsInt32OrDefault(int.MinValue));
            //Assert.Equal(-1, "xxxx".AsInt32OrDefault(NumberStyles.Currency, -1));
            //Assert.Equal(int.MaxValue, "xxxx".AsInt32OrDefault(NumberStyles.Currency, numFormat, int.MaxValue));

            //Assert.Null("!00".AsInt32OrNull());
            //Assert.Null(@"QQQ,999\".AsInt32OrNull(NumberStyles.Currency));
            //Assert.Null("9!9999!9999¥ë¥Ô©`".AsInt32OrNull(NumberStyles.Currency, numFormat));
        }
        [Fact]
        public void StringIsTest()
        {
            Assert.True("12345".IsInt());
            //Assert.False(@"12,345\".IsInt());
            Assert.True("12345".IsInt());
            //Assert.False("1`2345`6789¥ë¥Ô©`".IsInt());

            //Assert.Equal(999, "999".AsInt32OrNull());
            //Assert.Equal(999999, @"999,999\".AsInt32OrNull(NumberStyles.Currency));
            //Assert.Equal(999999999, "9`9999`9999¥ë¥Ô©`".AsInt32OrNull(NumberStyles.Currency, numFormat));

            //Assert.Equal(default(int), "xxxx".AsInt32OrDefault());
            //Assert.Equal(default(int), "xxxx".AsInt32OrDefault(NumberStyles.Currency));
            //Assert.Equal(default(int), "xxxx".AsInt32OrDefault(NumberStyles.Currency, numFormat));

            //Assert.Equal(int.MinValue, "xxxx".AsInt32OrDefault(int.MinValue));
            //Assert.Equal(-1, "xxxx".AsInt32OrDefault(NumberStyles.Currency, -1));
            //Assert.Equal(int.MaxValue, "xxxx".AsInt32OrDefault(NumberStyles.Currency, numFormat, int.MaxValue));

            //Assert.Null("!00".AsInt32OrNull());
            //Assert.Null(@"QQQ,999\".AsInt32OrNull(NumberStyles.Currency));
            //Assert.Null("9!9999!9999¥ë¥Ô©`".AsInt32OrNull(NumberStyles.Currency, numFormat));
        }
        //[TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        //public void AsInt32Test_2()
        //{
        //    "XXXXX".AsInt32();
        //}
        //[TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        //public void AsInt32Test_3()
        //{
        //    @"XXXXX\".AsInt32(NumberStyles.Currency);
        //}
        //[TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        //public void AsInt32Test_4()
        //{
        //    "ABCDE".AsInt32(CultureInfo.CurrentCulture.NumberFormat);
        //}
        //[TestMethod]
        //[ExpectedException(typeof(System.FormatException))]
        //public void AsInt32Test_5()
        //{
        //    var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        //    numFormat.CurrencySymbol = "¥ë¥Ô©`";
        //    numFormat.CurrencyGroupSeparator = "`";
        //    numFormat.CurrencyGroupSizes = new[] { 4 };
        //    "123,456,789G".AsInt32(NumberStyles.Currency, numFormat);
        //}
    }
}