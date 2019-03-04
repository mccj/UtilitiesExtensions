﻿using System.Diagnostics;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// 程序集扩展操作类
    /// </summary>
    public static class AssemblyExtensions
    {
        public static FileVersionInfo GetFileVersionInfo(this Assembly assembly)
        {
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return info;
        }
        /// <summary>
        /// 获取程序集的文件版本
        /// </summary>
        public static Version GetFileVersion(this Assembly assembly)
        {
            var info = GetFileVersionInfo(assembly);
            return new Version(info.FileVersion);
        }

        /// <summary>
        /// 获取程序集的产品版本
        /// </summary>
        public static Version GetProductVersion(this Assembly assembly)
        {
            var info = GetFileVersionInfo(assembly);
            return new Version(info.ProductVersion);
        }
        public static AssemblyName GetNameSafe(this Assembly assembly)
        {
            return new AssemblyName(assembly.FullName);
        }

    }
}
