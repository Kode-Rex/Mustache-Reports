using System;
using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Report.Word;
using Mustache.Reports.Boundry.Report;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain
{
    public class RenderWordUseCase : IRenderWordUseCase
    {
        private readonly IReportGateway _wordGateway;

        public RenderWordUseCase(IReportGateway wordGateway)
        {
            _wordGateway = wordGateway ?? throw new ArgumentNullException(nameof(wordGateway));
        }

        public void Execute(RenderWordInput inputInput, IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter)
        {
            var result = _wordGateway.CreateWordReport(inputInput);

            if (result.HasErrors())
            {
                RespondWithErrors(presenter, result);
                return;
            }

            RespondWithFile(inputInput, presenter, result);
        }

        private void RespondWithFile(RenderWordInput inputInput, IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter, RenderedDocummentOutput result)
        {
            var reportMessage = new WordFileOutput(inputInput.ReportName, result.FetchDocumentAsByteArray());

            presenter.Respond(reportMessage);
        }

        private void RespondWithErrors(IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter, RenderedDocummentOutput result)
        {
            var errors = new ErrorOutput();
            errors.AddErrors(result.ErrorMessages);
            presenter.Respond(errors);
        }
    }
}
