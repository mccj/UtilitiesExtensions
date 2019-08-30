using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace System.Linq
{
    public static class DataRowExtensions
    {
        public static T Field<T>(this DataRow row, DataColumn column)
        {
            return System.Data.DataRowExtensions.Field<T>(row, column);
        }

        public static T Field<T>(this DataRow row, DataColumn column, DataRowVersion version)
        {
            return System.Data.DataRowExtensions.Field<T>(row, column, version);
        }

        public static T Field<T>(this DataRow row, int columnIndex)
        {
            return System.Data.DataRowExtensions.Field<T>(row, columnIndex);
        }

        public static T Field<T>(this DataRow row, int columnIndex, DataRowVersion version)
        {
            return System.Data.DataRowExtensions.Field<T>(row, columnIndex, version);
        }

        public static T Field<T>(this DataRow row, string columnName)
        {
            return System.Data.DataRowExtensions.Field<T>(row, columnName);
        }

        public static T Field<T>(this DataRow row, string columnName, DataRowVersion version)
        {
            return System.Data.DataRowExtensions.Field<T>(row, columnName, version);
        }

        public static void SetField<T>(this DataRow row, DataColumn column, T value)
        {
             System.Data.DataRowExtensions.SetField(row, column, value);
        }

        public static void SetField<T>(this DataRow row, int columnIndex, T value)
        {
            System.Data.DataRowExtensions.SetField(row, columnIndex, value);
        }

        public static void SetField<T>(this DataRow row, string columnName, T value)
        {
            System.Data.DataRowExtensions.SetField(row, columnName, value);
        }
    }
}
