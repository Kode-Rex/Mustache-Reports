using Simple.Report.Server.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Simple.Report.Server.Domain.UseCases
{
    public interface IRenderReportUseCase : IUseCase<CreateReportMessage, IFileOutput>
    {
    }
}
