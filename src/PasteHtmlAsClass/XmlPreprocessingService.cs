using System;
using System.ComponentModel.Composition;

namespace PasteHtmlAsClass
{
    [Export(typeof(IXmlPreprocessingService))]
    public class XmlPreprocessingService : IXmlPreprocessingService
    {
        public string MatchTable(string html)
        {
            throw new NotImplementedException();
        }
    }
}