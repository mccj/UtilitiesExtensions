﻿using System.Text;

namespace System.Linq
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] source, Encoding encoding)
        {
            return encoding.GetString(source);
        }

        public static string GetString(this byte[] source)
        {
            return Encoding.Default.GetString(source);
        }
    }

}
