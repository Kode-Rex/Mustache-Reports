using System;
using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Report;
using Mustache.Reports.Boundary.Report.Word;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain.Word
{
    public class RenderWordUseCase : IRenderWordUseCase
    {
        private readonly IReportGateway _wordGateway;

        public RenderWordUseCase(IReportGateway wordGateway)
        {
            _wordGateway = wordGateway ?? throw new ArgumentNullException(nameof(wordGateway));
        }

        public void Execute(RenderWordInput inputTo, 
                            IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter)
        {
            var result = _wordGateway.CreateWordReport(inputTo);

            if (result.HasErrors())
            {
                Respond_With_Errors(presenter, result);
                return;
            }

            Respond_With_File(inputTo, presenter, result);
        }

        private void Respond_With_File(RenderWordInput inputInput, 
                                       IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter, 
                                       RenderedDocumentOutput result)
        {
            var reportMessage = new WordFileOutput(inputInput.ReportName, result.FetchDocumentAsByteArray());

            presenter.Respond(reportMessage);
        }

        private void Respond_With_Errors(IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter, 
                                         RenderedDocumentOutput result)
        {
            var errors = new ErrorOutput();
            errors.AddErrors(result.ErrorMessages);
            presenter.Respond(errors);
        }
    }
}
