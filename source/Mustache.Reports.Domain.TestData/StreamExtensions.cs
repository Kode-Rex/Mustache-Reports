using System;
using System.IO;

namespace Mustache.Reports.Domain.TestData
{
    public static class StreamExtensions
    {
        public static string ToBase64String(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return string.Join(
                    "",
                    "data:text/plain;base64,",
                    Convert.ToBase64String(ms.ToArray())
                );
            }
        }
    }
}