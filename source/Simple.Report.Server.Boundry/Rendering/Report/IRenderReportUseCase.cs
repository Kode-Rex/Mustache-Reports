using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Simple.Report.Server.Boundry.Rendering.Report
{
    public interface IRenderReportUseCase : IUseCase<RenderReportInput, IFileOutput>
    {
    }
}
