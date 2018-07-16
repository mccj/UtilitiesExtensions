using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// 类型<see cref="Type"/>辅助扩展方法类
    /// </summary>
    public static class StreamExtensions
    {
        ///// <summary>
        ///// 判断类型是否为Nullable类型
        ///// </summary>
        ///// <param name="type"> 要处理的类型 </param>
        ///// <returns> 是返回True，不是返回False </returns>
        //public static string ToMd5Hash(this System.IO.Stream stream)
        //{
        //    return Orchard.Utility.Secutiry.HashHelper.GetMd5(stream);
        //}
        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this System.IO.Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, IO.SeekOrigin.Begin);
            return bytes;
        }
        ///// <summary>
        ///// 将 byte[] 转成 Stream
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <returns></returns>

        //public Stream BytesToStream(byte[] bytes)
        //{
        //    Stream stream = new MemoryStream(bytes);
        //    return stream;
        //}
    }
}
