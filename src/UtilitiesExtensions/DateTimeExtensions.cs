namespace System.Linq
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateStringFormat(this DateTime input)
        {
            return input.ToString("dd-MM-yyyy");
        }

        public static string ToDateTimeStringFormat(this DateTime input)
        {
            return input.ToString("dd-MMM-yyyy hh:mm tt");
        }
    }
}
