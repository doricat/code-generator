using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasteHtmlAsClass
{
    [Export(typeof(IXmlProcessorService))]
    public class XmlProcessorService : IXmlProcessorService
    {
        public string MatchTable(string html)
        {
            var rows = MatchRows(html);
            return rows.Any() ? $"<table>{string.Join("", rows)}</table>" : null;
        }

        protected IList<string> MatchRows(string html)
        {
            var matches = Regex.Matches(html, @"<(tr*)\b[^>]*>(.*?)</\1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var list = matches.Cast<Match>().Select(x => RemoveEscapedCharacters(RemovePrefix(RemoveAttributes(RemoveComment(x.Value))))).ToList();
            return list;
        }

        protected string RemoveAttributes(string xml)
        {
            var text = xml;
            var matches = Regex.Matches(xml, @"<[a-z][a-z0-9]*[^<>]*>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match item in matches)
            {
                var match = Regex.Match(item.Value, @"<[a-z][a-z0-9]*(:[a-z][a-z0-9]*)?", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    text = text.Replace(item.Value, $"{match.Value}>");
                }
            }

            return text;
        }

        protected string RemovePrefix(string xml)
        {
            var text = xml;
            var matches = Regex.Matches(xml, @"</?[a-z][a-z0-9]*:", RegexOptions.IgnoreCase);
            foreach (Match item in matches)
            {
                text = text.Replace(item.Value, item.Value.IndexOf('/') > 0 ? "</" : "<");
            }

            return text;
        }

        protected string RemoveComment(string xml)
        {
            var text = xml;
            var matches = Regex.Matches(xml, @"<!--.*?-->");
            foreach (Match item in matches)
            {
                text = text.Replace(item.Value, "");
            }

            return text;
        }

        protected string RemoveEscapedCharacters(string xml)
        {
            var text = xml.Replace("&nbsp;", " ");
            return text;
        }
    }
}