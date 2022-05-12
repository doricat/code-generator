using System;
using System.ComponentModel.Composition;

namespace PasteHtmlAsClass
{
    [Export(typeof(IXmlProcessorService))]
    public class XmlProcessorService : IXmlProcessorService
    {
        public string MatchTable(string html)
        {
            throw new NotImplementedException();
        }
    }
}