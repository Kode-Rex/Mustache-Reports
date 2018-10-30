using StoneAge.CleanArchitecture.Domain;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Boundry.Pdf
{
    public interface IRenderDocxToPdfUseCase : IUseCase<RenderPdfInput, IFileOutput>
    {
    }
}