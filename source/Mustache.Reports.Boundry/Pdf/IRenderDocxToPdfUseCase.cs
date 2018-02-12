using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Boundry.Pdf
{
    public interface IRenderDocxToPdfUseCase : IUseCase<RenderPdfInput, IFileOutput>
    {
    }
}