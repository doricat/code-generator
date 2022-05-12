using System;
using System.Collections.Generic;
using System.Linq;

namespace PasteHtmlAsClass
{
    public class Table
    {
        public Table(IList<Row> rows)
        {
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));
        }

        public IList<Row> Rows { get; }

        public string GenerateCode()
        {
            var name = Guid.NewGuid().ToString("N").Substring(0, 6);
            var properties = Rows.Select(x => x.GenerateCode()).ToList();
            return $@"public class _{name}
{{
{string.Join("\r\n", properties)}
}}";
        }
    }
}