using System.Collections.Generic;

namespace EntityTypeConfigurationGenerator
{
    public static class DefaultTypeMapping
    {
        private static readonly IDictionary<string, string> MySqlTypeMapping = new Dictionary<string, string>
        {
            {"sbyte", "smallint"},
            {"byte", "smallint"},
            {"short", "smallint"},
            {"ushort", "int"},
            {"int", "int"},
            {"uint", "bigint"},
            {"long", "bigint"},
            {"ulong", "bigint"},
            {"char", "char(1)"},
            {"float", "decimal(14,4)"},
            {"double", "decimal(14,4)"},
            {"bool", "TINYINT"},
            {"decimal", "decimal(14,4)"},
            {"string", "varchar(50)"}
        };

        public static string GetType(string keyword)
        {
            if (MySqlTypeMapping.ContainsKey(keyword))
            {
                return MySqlTypeMapping[keyword];
            }

            return "unknown";
        }
    }
}