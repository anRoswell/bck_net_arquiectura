using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Core.Tools
{
    public static class BaseMultiplesTables
    {
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            List<string> columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();

            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();

                foreach (PropertyInfo pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception e) {
                            throw new BusinessException($"Error: {e.Message}");
                        }
                    }
                }
                return objT;
            }).ToList();
        }
    }
}
