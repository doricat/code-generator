using System.Collections.Generic;

namespace PasteHtmlAsClass
{
    public class Row
    {
        public string Identifier { get; set; }

        public string Type { get; set; } = "object";

        public string Summary { get; set; }

        public string Remarks { get; set; }

        public string GenerateCode()
        {
            var lines = new List<string>();
            if (!string.IsNullOrEmpty(Summary))
            {
                lines.Add($@"/// <summary>
/// {Summary}
/// </summary>");
            }

            if (!string.IsNullOrEmpty(Remarks))
            {
                lines.Add($"/// <remarks>{Remarks}</remarks>");
            }

            lines.Add($"public {Type} {Identifier} {{ get; set; }}");
            return string.Join("\r\n", lines);
        }
    }
}