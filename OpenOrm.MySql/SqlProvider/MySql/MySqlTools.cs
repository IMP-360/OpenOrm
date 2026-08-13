using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace OpenOrm.SqlProvider.MySql
{
    public static class MySqlTools
    {
        
        public static string FormatValueToString(object value)
        {
            if (value != null)
            {
                string formattedValue = "NULL";
                string ptype = value.GetType().Name.ToLower();
                switch (ptype)
                {
                    case "string":
                        formattedValue = $"\"{value.ToString().Replace("\"", "'")}\"";
                        break;
                    case "char":
                        formattedValue = $"'{Convert.ToString(value)}'";
                        break;
                    case "long":
                    case "int64":
                    case "int":
                    case "int32":
                    case "int16":
                    case "short":
                    case "uint16":
                    case "ushort":
                    case "uint32":
                    case "uint":
                    case "uint64":
                    case "ulong":
                    case "byte":
                    case "sbyte":
                        formattedValue = $"{value}";
                        break;
                    case "decimal":
                    case "double":
                    case "float":
                    case "single":
                        string decsep = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
                        formattedValue = value.ToString().Replace(".", decsep).Replace(",", decsep);
                        break;
                    case "datetime":
                        //formattedValue = $"\"{(DateTime)value:dd/MM/yyyy HH:mm:ss}\"";
                        string ShortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                        string LongTimePattern = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
                        formattedValue = $"'{((DateTime)value).ToString($"{ShortDatePattern} {LongTimePattern}")}'";
                        break;
                    case "bool":
                    case "boolean":
                        string bv = (bool)value ? "1" : "0";
                        formattedValue = $"{bv}";
                        break;
                    case "object":
                        formattedValue = $"\"{value}\"";
                        break;
                }

                return formattedValue;
            }
            else
            {
                return "NULL";
            }
        }


        public static string ToStringType(SqlDbType dbtype, int size = 0, int scale = 16, int precision = 2)
        {
            switch (dbtype)
            {
                default:
                case SqlDbType.NVarChar:
                    if (size == -1) return "LONGTEXT";
                    if (size > 0) return $"NVARCHAR({size})";
                    else return "TEXT";
                case SqlDbType.VarChar:
                    if (size == -1) return "LONGTEXT";
                    if (size > 0) return $"VARCHAR({size})";
                    else return "TEXT";
                case SqlDbType.TinyInt:
                    return "TINYINT";
                case SqlDbType.SmallInt:
                    return "SMALLINT";
                case SqlDbType.Int:
                    return "INT";
                case SqlDbType.BigInt:
                    return "BIGINT";
                case SqlDbType.Date:
                    return "DATE";
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                    return "DATETIME";
                case SqlDbType.Time:
                case SqlDbType.Timestamp:
                    return "TIME";
                case SqlDbType.Bit:
                    return "BIT";
                case SqlDbType.Binary:
                case SqlDbType.VarBinary:
                    if (size > 0) return $"VARBINARY({size})";
                    else return "VARBINARY(8000)";
                case SqlDbType.NChar:
                    return size > 0 ? $"NCHAR({size})" : "NCHAR(1)";
                case SqlDbType.Char:
                    return size > 0 ? $"CHAR({size})" : "CHAR(1)";
                case SqlDbType.Decimal:
                    if (scale > 0 && precision > 0) return $"DECIMAL({scale},{precision})";
                    else if (scale > 0) return $"DECIMAL({scale},2)";
                    else if (precision > 0) return $"DECIMAL(16,{precision})";
                    else return "DECIMAL(16,2)";
                case SqlDbType.Float:
                    return "DOUBLE";
                case SqlDbType.Real:
                    return "FLOAT";
                case SqlDbType.UniqueIdentifier:
                    return "CHAR(36)";
            }
        }

        public static string ToStringType(PropertyInfo pi, int size = 0, int scale = 16, int precision = 2)
        {
            return ToStringType(OpenOrmTools.ToSqlDbType(pi), size, scale, precision);
        }

        public static string ToStringType(FieldInfo fi, int size = 0, int scale = 16, int precision = 2)
        {
            return ToStringType(OpenOrmTools.ToSqlDbType(fi), size, scale, precision);
        }

        public static string ToStringType(Type type, int size = 0, int scale = 16, int precision = 2)
        {
            return ToStringType(OpenOrmTools.ToSqlDbType(type), size, scale, precision);
        }

        public static string ToStringType<T>(int size = 0, int scale = 16, int precision = 2)
        {
            return ToStringType(typeof(T), size, scale, precision);
        }

        public static SqlDbType DetectParameterType(object value)
        {
            if (value is string)
                return SqlDbType.NVarChar;
            else if (value is ushort || value is int)
                return SqlDbType.Int;
            else if (value is uint || value is long || value is ulong)
                return SqlDbType.BigInt;
            else if (value is byte || value is sbyte)
                return SqlDbType.TinyInt;
            else if (value is short)
                return SqlDbType.SmallInt;
            else if (value is bool)
                return SqlDbType.Bit;
            else if (value is DateTime)
                return SqlDbType.DateTime;
            else if (value is byte[])
                return SqlDbType.VarBinary;
            else if (value is char)
                return SqlDbType.NChar;
            else if (value is decimal)
                return SqlDbType.Decimal;
            else if (value is float)
                return SqlDbType.Real;
            else if (value is double)
                return SqlDbType.Float;
            else if (value is TimeSpan)
                return SqlDbType.Timestamp;
            else
                return SqlDbType.NVarChar;
        }
    }
}
