using StoneAge.CleanArchitecture.Domain;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Boundary.Pdf
{
    public interface IRenderDocxToPdfUseCase : IUseCase<RenderPdfInput, IFileOutput>
    {
    }
}