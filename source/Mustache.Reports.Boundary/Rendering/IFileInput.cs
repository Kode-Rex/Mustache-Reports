using System.IO;

namespace Mustache.Reports.Boundary.Rendering
{
    public interface IFileInput
    {
        Stream GetStream();
    }
}