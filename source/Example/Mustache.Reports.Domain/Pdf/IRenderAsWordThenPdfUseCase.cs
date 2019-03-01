using Mustache.Reports.Boundary.Report.Word;
using StoneAge.CleanArchitecture.Domain;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain.Pdf
{
    public interface IRenderAsWordThenPdfUseCase : IUseCase<RenderWordInput, IFileOutput>
    {
    }
}
