namespace System.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int16Extensions
    {
        public static string PadLeft(this Int16 value, int totalWidth) => Convert.ToString(value).PadLeft(totalWidth);
        public static string PadLeft(this Int16 value, int totalWidth, char paddingChar) => Convert.ToString(value).PadLeft(totalWidth, paddingChar);
        public static string PadRight(this Int16 value, int totalWidth) => Convert.ToString(value).PadRight(totalWidth);
        public static string PadRight(this Int16 value, int totalWidth, char paddingChar) => Convert.ToString(value).PadRight(totalWidth, paddingChar);

    }
}
