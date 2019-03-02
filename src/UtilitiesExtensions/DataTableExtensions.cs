using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace System.Linq
{
    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();
                foreach (DataRow item in table.AsEnumerable())
                {
                    T val = new T();
                    PropertyInfo[] properties = val.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        try
                        {
                            PropertyInfo property = val.GetType().GetProperty(propertyInfo.Name);
                            if (Nullable.GetUnderlyingType(property.PropertyType) != (Type)null && !(item[propertyInfo.Name] is DBNull))
                            {
                                Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                                property.SetValue(val, Convert.ChangeType(item[propertyInfo.Name], underlyingType), null);
                            }
                            else
                            {
                                property.SetValue(val, Convert.ChangeType(item[propertyInfo.Name], property.PropertyType), null);
                            }
                        }
                        catch
                        {
                        }
                    }
                    list.Add(val);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
