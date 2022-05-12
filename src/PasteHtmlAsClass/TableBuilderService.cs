using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

namespace PasteHtmlAsClass
{
    [Export(typeof(ITableBuilderService))]
    public class TableBuilderService : ITableBuilderService
    {
        public Table Build(string xml)
        {
            if (xml == null)
            {
                return null;
            }

            var doc = XDocument.Parse(xml);
            if (doc.Root == null)
            {
                return null;
            }

            var rows = doc.Root.Elements()
                .Where(x => x.Name.LocalName.Equals("tr", StringComparison.OrdinalIgnoreCase))
                .Select(x => Parse(x))
                .Where(x => x != null)
                .ToList();
            return new Table(rows);
        }

        protected Row Parse(XElement element)
        {
            var index = 0;
            var row = new Row();
            foreach (var item in element.Elements())
            {
                if (index >= DefaultColumnMapping.Mapping.Count)
                {
                    break;
                }

                if (item.Name.LocalName.Equals("td", StringComparison.OrdinalIgnoreCase))
                {
                    DefaultColumnMapping.Mapping[index](row, item.Value);
                    index++;
                }
            }

            return string.IsNullOrEmpty(row.Identifier) ? null : row;
        }
    }
}