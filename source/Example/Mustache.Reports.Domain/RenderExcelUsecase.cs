using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Report;
using Mustache.Reports.Boundary.Report.Excel;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain
{
    public class RenderExcelUseCase : IRenderExcelUseCase
    {
        private readonly IReportGateway _reportGateway;

        public RenderExcelUseCase(IReportGateway reportGateway)
        {
            _reportGateway = reportGateway;
        }

        public void Execute(RenderExcelInput inputInput, 
                            IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter)
        {
            var result = _reportGateway.CreateExcelReport(inputInput);

            if (result.HasErrors())
            {
                Respond_With_Errors(presenter, result);
                return;
            }

            Respond_With_File(inputInput, presenter, result);
        }

        private static void Respond_With_File(RenderExcelInput input, 
                                              IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter, 
                                              RenderedDocumentOutput result)
        {
            var reportMessage = new WordFileOutput(input.ReportName, result.FetchDocumentAsByteArray());

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
