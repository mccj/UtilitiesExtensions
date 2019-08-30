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
            var s1 = DynamicQueryableExtensions.OrderBy(tms, "Age").FirstOrDefault();
            var s2 = DynamicQueryableExtensions.OrderBy(tms, "Age desc").FirstOrDefault();
            var s3 = DynamicQueryableExtensions.OrderBy(tms, "", "Age").FirstOrDefault();
            var s4 = DynamicQueryableExtensions.OrderBy(tms, "", "Age desc").FirstOrDefault();
            var s5 = DynamicQueryableExtensions.OrderBy(tms, "", f => f.OrderBy(ff => ff.Age)).FirstOrDefault();
            var s6 = DynamicQueryableExtensions.OrderBy(tms, "", f => f.OrderByDescending(ff => ff.Age)).FirstOrDefault();
            var s11 = DynamicQueryableExtensions.OrderBy(tms.AsQueryable(), "Age").FirstOrDefault();
            var s12 = DynamicQueryableExtensions.OrderBy(tms.AsQueryable(), "Age desc").FirstOrDefault();
            var s13 = DynamicQueryableExtensions.OrderBy(tms.AsQueryable(), "", "Age").FirstOrDefault();
            var s14 = DynamicQueryableExtensions.OrderBy(tms.AsQueryable(), "", "Age desc").FirstOrDefault();
            var s15 = DynamicQueryableExtensions.OrderBy(tms.AsQueryable(), "", f => f.OrderBy(ff => ff.Age)).FirstOrDefault();
            var s16 = DynamicQueryableExtensions.OrderBy(tms.AsQueryable(), "", f => f.OrderByDescending(ff => ff.Age)).FirstOrDefault();
            var s17 = DynamicQueryableExtensions.OrderBy(tms.AsQueryable(), "", f => f.Age).FirstOrDefault();

            Assert.Equal(5, s1.Age);
            Assert.Equal(6, s2.Age);
            Assert.Equal(5, s3.Age);
            Assert.Equal(6, s4.Age);
            Assert.Equal(5, s5.Age);
            Assert.Equal(6, s6.Age);
            Assert.Equal(5, s11.Age);
            Assert.Equal(6, s12.Age);
            Assert.Equal(5, s13.Age);
            Assert.Equal(6, s14.Age);
            Assert.Equal(5, s15.Age);
            Assert.Equal(6, s16.Age);
            Assert.Equal(5, s17.Age);
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
        public void WhereTest()
        {
            var s1 = DynamicQueryableExtensions.Where(tms, "Age==5").FirstOrDefault();
            var s2 = DynamicQueryableExtensions.Where(tms, "", "Age==6").FirstOrDefault();
            var s3 = DynamicQueryableExtensions.Where(tms, "Age==5", "Age==6").FirstOrDefault();
            var s4 = DynamicQueryableExtensions.Where(tms, "Age==@0", 6).FirstOrDefault();
            var s11 = DynamicQueryableExtensions.Where(tms.AsQueryable(), "Age==5").FirstOrDefault();
            var s11_ = DynamicQueryableExtensions.Where(tms.AsQueryable(), "1==1").FirstOrDefault();
            var s12 = DynamicQueryableExtensions.Where(tms.AsQueryable(), "", "Age==6").FirstOrDefault();
            var s13 = DynamicQueryableExtensions.Where(tms.AsQueryable(), "Age==5", "Age==6").FirstOrDefault();
            var s14 = DynamicQueryableExtensions.Where(tms.AsQueryable(), "Age==@0", 6).FirstOrDefault();

            Assert.Equal(5, s1.Age);
            Assert.Equal(6, s2.Age);
            Assert.Equal(5, s3.Age);
            Assert.Equal(6, s4.Age);
            //Assert.Equal(5, s5.Age);
            //Assert.Equal(6, s6.Age);
            Assert.Equal(5, s11.Age);
            Assert.Equal(5, s11_.Age);
            Assert.Equal(6, s12.Age);
            Assert.Equal(5, s13.Age);
            Assert.Equal(6, s14.Age);
            //Assert.Equal(5, s15.Age);
            //Assert.Equal(6, s16.Age);
            //Assert.Equal(5, s17.Age);
        }
    }

    public class Tm
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}