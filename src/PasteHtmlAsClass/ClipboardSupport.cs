using System.Windows.Forms;

namespace PasteHtmlAsClass
{
    public static class ClipboardSupport
    {
        public static string GetClipboardData()
        {
            try
            {
                var data = Clipboard.GetDataObject();
                if (data != null)
                {
                    if (data.GetDataPresent(DataFormats.Html))
                    {
                        var text = data.GetData(DataFormats.Html);
                        return text.ToString();
                    }
                }
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                // Ignored
            }

            return null;
        }
    }
}