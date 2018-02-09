using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Rendering;
using Mustache.Reports.Boundry.Rendering.Report;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain
{
    public class RenderReportUseCase : IRenderReportUseCase
    {
        private readonly IReportRepository _reportRepository;

        public RenderReportUseCase(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public void Execute(RenderReportInput inputInput, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter)
        {
            var result = _reportRepository.CreateReport(inputInput);

            if (result.HasErrors())
            {
                RespondWithErrors(presenter, result);
                return;
            }

            RespondWithFile(inputInput, presenter, result);
        }

        private static void RespondWithFile(RenderReportInput inputInput, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter, RenderedDocummentOutput result)
        {
            var reportMessage = new WordFileOutput(inputInput.ReportName, result.FetchDocumentAsByteArray());

            presenter.Respond(reportMessage);
        }

        private void RespondWithErrors(IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter, RenderedDocummentOutput result)
        {
            var errors = new ErrorOutputMessage();
            errors.AddErrors(result.ErrorMessages);
            presenter.Respond(errors);
        }
    }
}
