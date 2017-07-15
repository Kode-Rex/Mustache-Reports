using Simple.Report.Server.Domain.Messages;
using Simple.Report.Server.Domain.Repositories;
using Simple.Report.Server.Domain.UseCases;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Simple.Report.Server.UseCase
{
    public class RenderReportUseCase : IRenderReportUseCase
    {
        private readonly IReportRepository _reportRepository;

        public RenderReportUseCase(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public void Execute(CreateReportMessage inputMessage, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter)
        {
            var result = _reportRepository.CreateReport(inputMessage);

            var reportMessage = new InMemoryWordFileOutputMessage(inputMessage.ReportName,result.FetchReportAsByteArray());
            presenter.Respond(reportMessage);
        }
    }
}
