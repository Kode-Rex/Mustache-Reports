using Simple.Report.Server.Boundry.ReportRendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Simple.Report.Server.Domain
{
    public class RenderReportUseCase : IRenderReportUseCase
    {
        private readonly IReportRepository _reportRepository;

        public RenderReportUseCase(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public void Execute(RenderReportInputMessage inputInputMessage, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter)
        {
            var result = _reportRepository.CreatePdfReport(inputInputMessage);

            if (result.HasErrors())
            {
                RespondWithErrors(presenter, result);
                return;
            }

            RespondWithFile(inputInputMessage, presenter, result);
        }

        private static void RespondWithFile(RenderReportInputMessage inputInputMessage, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter,
            RenderedReportOutputMessage result)
        {
            var reportMessage = new WordFileOutputMessage(inputInputMessage.ReportName,
            result.FetchReportAsByteArray());
            presenter.Respond(reportMessage);
        }

        private void RespondWithErrors(IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter, RenderedReportOutputMessage result)
        {
            var errors = new ErrorOutputMessage();
            errors.AddError(result.ErrorMessages);
            presenter.Respond(errors);
        }
    }
}
