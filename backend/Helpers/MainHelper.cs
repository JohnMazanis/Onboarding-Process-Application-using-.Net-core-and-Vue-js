using backend.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class MainHelper
    {
        public static string GetConnectionString()
        {
            string conString = @"Data Source=.\SQLEXPRESS;Initial Catalog=enboard;User ID=sa;Password=mazanissa";
            return conString;
        }

        private static IEnumerable<PropertyInfo> OrderPropertiesByCustomAttribute<T>()
        {
            var tp = typeof(T);
            var props = tp.GetProperties().Select(x => new
            {
                Property = x,
                Attribute = (PropertyIndex)Attribute.GetCustomAttribute(x, typeof(PropertyIndex), true)
            })
            .OrderBy(x => x.Attribute != null ? x.Attribute.Index : -1)
            .Select(x => x.Property);
            return props;
        }

        private static string GetStoredProcTempTableInsertLoop<T>(List<T> items)
        {
            var props = OrderPropertiesByCustomAttribute<T>();
            var loop = new StringBuilder();
            foreach (var item in items)
            {
                var values = new List<string>();
                foreach (var p in props)
                {
                    Type pt = p.PropertyType;
                    Type npt = Nullable.GetUnderlyingType(pt) ?? pt;

                    var nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";

                    var val = item.GetType().GetProperty(p.Name).GetValue(item, null);
                    var valStr = string.Empty;
                    if (npt == typeof(DateTime))
                        valStr = val != null ? $"'{Convert.ToDateTime(val).ToString("yyyy-MM-dd")}'" : "NULL";
                    else if (npt == typeof(int))
                        valStr = val != null ? val.ToString() : "NULL";
                    else if (npt == typeof(double))
                        valStr = val != null ? Convert.ToDouble(val).ToString(nfi) : "NULL";
                    else
                        valStr = val != null ? $"'{val.ToString().Replace("'", "''")}'" : "NULL";

                    values.Add(valStr);
                }

                loop.AppendFormat(" UNION SELECT {0} ", string.Join(", ", values));
            }
            return loop.ToString();
        }

        private static string DeclareStoredProcTempTableSchemaByType<T>()
        {
            var tp = typeof(T);
            var props = OrderPropertiesByCustomAttribute<T>();
            var schema = new StringBuilder();
            schema.Append("SELECT ");
            var columns = new List<string>();
            foreach (var p in props)
            {
                Type pt = p.PropertyType;
                Type npt = Nullable.GetUnderlyingType(pt) ?? pt;

                if (npt != typeof(string) && npt != typeof(int) && npt != typeof(double) && npt != typeof(DateTime))
                    throw new ArgumentException($"Property {p.Name} of type {tp.Name} should be of type string, int, double or DateTime.");

                if (npt == typeof(int) || npt == typeof(double))
                    columns.Add(string.Format("9999 AS {0}", p.Name));
                else if (npt == typeof(string))
                    columns.Add(string.Format("'' AS {0}", p.Name));
                else if (npt == typeof(DateTime))
                    columns.Add(string.Format("GETDATE() AS {0}", p.Name));
            }
            schema.Append(string.Join(", ", columns));
            schema.Append(" WHERE 1=0 ");
            return schema.ToString();
        }

        private static string GetStoredProcTempTableColumnsByType<T>()
        {
            var tp = typeof(T);
            var props = OrderPropertiesByCustomAttribute<T>();
            var columns = new List<string>();
            foreach (var p in props)
                columns.Add(p.Name);

            return string.Join(", ", columns);
        }

        private static string GetStoredProcTempTableQuery<T>(string tempTableName, List<T> items)
        {
            var columns = GetStoredProcTempTableColumnsByType<T>();
            var schema = DeclareStoredProcTempTableSchemaByType<T>();
            var loop = GetStoredProcTempTableInsertLoop<T>(items);
            return string.Format(@"IF OBJECT_ID('tempdb..#{0}') IS NOT NULL DROP TABLE #{0}; SELECT {3} INTO #{0} FROM ({1} {2}) InnerTbl;", tempTableName, schema.ToString(), loop.ToString(), columns);
        }

        public static ProcessResponseObject SetStoredProcedureTempTable<T>(SqlConnection conn, string tempTableName, List<T> items, int? timeOut = null)
        {
            var resp = new ProcessResponseObject();

            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                resp.ErrorFound = true;
                resp.ErrorMessage = "tempTableName cannot be null or whitespace";
                return resp;
            }

            try
            {
                var tempTableCmd = new SqlCommand(GetStoredProcTempTableQuery(tempTableName, items), conn);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                if (timeOut != null)
                    tempTableCmd.CommandTimeout = (int)timeOut;

                tempTableCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conn.Close();
                resp.ErrorFound = true;
                resp.ErrorMessage = ex.Message;
            }

            return resp;
        }
    }
  }
