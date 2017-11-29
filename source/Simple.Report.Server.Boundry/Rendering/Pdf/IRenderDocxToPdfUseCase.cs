using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Simple.Report.Server.Boundry.Rendering.Pdf
{
    public interface IRenderDocxToPdfUseCase : IUseCase<RenderPdfInput, IFileOutput>
    {
    }
}