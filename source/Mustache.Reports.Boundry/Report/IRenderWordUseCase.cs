using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Boundry.Report
{
    public interface IRenderWordUseCase : IUseCase<RenderWordInput, IFileOutput>
    {
    }
}
