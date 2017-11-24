using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Simple.Report.Server.Boundry.ReportRendering
{
    public interface IRenderReportUseCase : IUseCase<RenderReportInputMessage, IFileOutput>
    {
    }
}
