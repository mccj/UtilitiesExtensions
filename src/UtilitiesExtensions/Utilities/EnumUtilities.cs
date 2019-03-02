// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace UtilitiesExtensions.Utility
{
    public class EnumUtilities<T>
    {
        public static string GetDescription(T enumValue, string defDesc)
        {
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
            if ((FieldInfo)null != field)
            {
                object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (customAttributes != null && customAttributes.Length != 0)
                {
                    return ((DescriptionAttribute)customAttributes[0]).Description;
                }
            }
            return defDesc;
        }

        public static string GetDescription(T enumValue)
        {
            return GetDescription(enumValue, string.Empty);
        }

        public static T FromDescription(string description)
        {
            Type typeFromHandle = typeof(T);
            FieldInfo[] fields = typeFromHandle.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (customAttributes != null && customAttributes.Length != 0)
                {
                    object[] array = customAttributes;
                    for (int j = 0; j < array.Length; j++)
                    {
                        DescriptionAttribute descriptionAttribute = (DescriptionAttribute)array[j];
                        if (descriptionAttribute.Description.Equals(description))
                        {
                            return (T)fieldInfo.GetValue(null);
                        }
                    }
                }
            }
            return default(T);
        }
    }
}
