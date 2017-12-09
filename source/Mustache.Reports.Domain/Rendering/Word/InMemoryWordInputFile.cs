using System;
using System.IO;
using System.Linq;
using Mustache.Reports.Boundary.Rendering.Word;

namespace Mustache.Reports.Domain.Rendering.Word
{
    public class InMemoryWordInputFile : IWordFileInput
    {
        private readonly string _dataUri;

        public InMemoryWordInputFile(string dataUri)
        {
            _dataUri = dataUri;
        }

        public Stream GetStream()
        {
            return new MemoryStream(DataUriToBytes());
        }

        private byte[] DataUriToBytes()
        {
            var base64Part = _dataUri
                .SkipWhile(c => c != ',')
                .Skip(1)
                .ToArray();

            return Convert.FromBase64CharArray(base64Part, 0, base64Part.Length);
        }
    }
}