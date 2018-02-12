using System;
using System.Collections.Generic;

namespace Mustache.Reports.Boundry
{
    public class RenderedDocummentOutput
    {
        public string Base64String { get; set; }
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
            return Convert.FromBase64String(Base64String);
        }
    }
}
