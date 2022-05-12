using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PasteHtmlAsClass
{
    public class DefaultColumnMapping
    {
        public static IDictionary<int, Action<Row, string>> Mapping = new Dictionary<int, Action<Row, string>>
        {
            {0, (x, y) => x.Identifier = y},
            {1, (x, y) =>
            {
                var match = Regex.Match(y, @"^[a-zA-Z]+\d?");
                if (match.Success)
                {
                    if (DefaultTypeMapping.Mapping.ContainsKey(match.Value))
                    {
                        x.Type = DefaultTypeMapping.Mapping[match.Value];
                    }
                    else if (DefaultTypeMapping.MappingFunc.ContainsKey(match.Value))
                    {
                        x.Type = DefaultTypeMapping.MappingFunc[match.Value](y);
                    }
                }
            }},
            {2, (x, y) => x.Summary = y},
            {3, (x, y) => x.Remarks = y}
        };
    }
}