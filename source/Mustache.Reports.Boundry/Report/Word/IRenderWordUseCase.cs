using StoneAge.CleanArchitecture.Domain;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Boundry.Report.Word
{
    public interface IRenderWordUseCase : IUseCase<RenderWordInput, IFileOutput>
    {
    }
}
