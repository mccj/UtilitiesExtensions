namespace System.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int32Extensions
    {
        public static string PadLeft(this int value, int totalWidth) => Convert.ToString(value).PadLeft(totalWidth);
        public static string PadLeft(this int value, int totalWidth, char paddingChar) => Convert.ToString(value).PadLeft(totalWidth, paddingChar);
        public static string PadRight(this int value, int totalWidth) => Convert.ToString(value).PadRight(totalWidth);
        public static string PadRight(this int value, int totalWidth, char paddingChar) => Convert.ToString(value).PadRight(totalWidth, paddingChar);
    }
}
