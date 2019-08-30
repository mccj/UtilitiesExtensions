using System.Collections.Generic;
using System.Data;
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


        public static EnumerableRowCollection<DataRow> AsEnumerable(this DataTable source)
        {
            return System.Data.DataTableExtensions.AsEnumerable(source);
        }

        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source) where T : DataRow
        {
            return System.Data.DataTableExtensions.CopyToDataTable(source);
        }

        public static void CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption options) where T : DataRow
        {
            System.Data.DataTableExtensions.CopyToDataTable(source, table, options);
        }

        public static void CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption options, FillErrorEventHandler errorHandler) where T : DataRow
        {
            System.Data.DataTableExtensions.CopyToDataTable(source, table, options, errorHandler);
        }

    }
}
