using System;
using System.Collections.Generic;
using System.IO;

namespace Mustache.Reports.Boundry
{
    public class RenderedDocummentOutput
    {
        public Stream DocumentStream { get; set; }
        public List<string> ErrorMessages { get; set; }

        public RenderedDocummentOutput()
        {
            ErrorMessages = new List<string>();
        }

        public bool HasErrors()
        {
            return ErrorMessages.Count > 0;
        }

        public byte[] FetchDocumentAsByteArray()
        {
            if (DocumentStream != null && DocumentStream.CanRead)
            {
                using (var stream = new MemoryStream())
                {
                    DocumentStream.CopyTo(stream);
                    return stream.ToArray();
                }
            }

            return new byte[0];
        }
    }
}
