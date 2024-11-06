namespace Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class DataTableExtenions
	{
        public static DataTable CreateDataTable(this Dictionary<string, Type> columnDefinitions)
        {
            var dt = new DataTable();
            foreach (var kvp in columnDefinitions)
            {
                dt.Columns.Add(kvp.Key, kvp.Value);
            }
            return dt;
        }

        public static void AddRowWithValues(this DataTable dt, Dictionary<string, object> columnValues)
        {
            var row = dt.NewRow();
            foreach (var kvp in columnValues)
            {
                row[kvp.Key] = kvp.Value ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
    }
}

