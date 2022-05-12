using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PasteHtmlAsClass
{
    public static class DefaultTypeMapping
    {
        private const string LongType = "long";
        private const string BoolType = "bool";
        private const string ShortType = "short";
        private const string DecimalType = "decimal";
        private const string IntType = "int";
        private const string ByteType = "byte";
        private const string FloatType = "float";
        private const string DoubleType = "double";
        private const string DateTimeOffsetType = "DateTimeOffset";
        private const string DateTimeType = "DateTime";
        private const string TimeSpanType = "TimeSpan";
        private const string StringType = "string";
        private const string ByteArrayType = "byte[]";

        public static readonly IDictionary<string, string> Mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"bigint", LongType},
            {"numeric", DecimalType},
            {"bit", BoolType},
            {"smallint", ShortType},
            {"decimal", DecimalType},
            {"smallmoney", DecimalType},
            {"int", IntType},
            {"tinyint", ByteType},
            {"money", DecimalType},
            {"float", FloatType},
            {"real", DoubleType},
            {"date",DateTimeType},
            {"datetimeoffset", DateTimeOffsetType},
            {"datetime2", DateTimeType},
            {"smalldatetime", DateTimeType},
            {"datetime", DateTimeType},
            {"time", TimeSpanType},
            {"timestamp", DateTimeOffsetType},
            {"char", StringType},
            {"varchar", StringType},
            {"varchar2", StringType},
            {"text", StringType},
            {"nchar", StringType},
            {"nvarchar", StringType},
            {"nvarchar2", StringType},
            {"ntext", StringType},
            {"binary", ByteArrayType},
            {"varbinary", ByteArrayType},
            {"image", ByteArrayType},
            {"blob", ByteArrayType},
            {"clob", ByteArrayType},
            {"nclob", ByteArrayType},
            {"bfile", ByteArrayType},
            {"long", ByteArrayType},
            {"raw", ByteArrayType}
        };

        public static readonly IDictionary<string, Func<string, string>> MappingFunc = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "number", x =>
                {
                    if (x.IndexOf(',') > 0)
                    {
                        return DecimalType;
                    }

                    return Regex.IsMatch(x, @"\d") ? IntType : LongType;
                }
            }
        };
    }
}