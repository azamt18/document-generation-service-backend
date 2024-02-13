using System.Text;

namespace API.Utility
{
    public static class PdfUtility
    {   
        public static bool IsPdfContent(byte[] content)
        {
            foreach (var encoding in new Encoding[]
            {
                Encoding.UTF8,
                Encoding.ASCII
            })
            {
                var allPdfFileStartsWithString = "%PDF-";
                var allPdfFileStartsWithStringBytes = encoding.GetBytes(allPdfFileStartsWithString);

                if (content.Length >= allPdfFileStartsWithStringBytes.Length)
                {
                    using (var fileStream = new MemoryStream(content))
                    {
                        var fileContentStartBytes = new byte[allPdfFileStartsWithStringBytes.Length];
                        fileStream.Read(fileContentStartBytes, 0, allPdfFileStartsWithStringBytes.Length);

                        if (fileContentStartBytes.SequenceEqual(allPdfFileStartsWithStringBytes))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
