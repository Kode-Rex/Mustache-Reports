using Simple.Report.Server.Domain.Messages.Input;
using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Simple.Report.Server.Domain.UseCases
{
    public interface IRenderReportUseCase : IUseCase<RenderReportInputMessage, IFileOutput>
    {
    }
}
