using System;
using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Pdf;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain
{
    public class RenderWordToPdfUseCase : IRenderDocxToPdfUseCase
    {
        private readonly IPdfGateway _pdfGateway;

        public RenderWordToPdfUseCase(IPdfGateway pdfGateway)
        {
            _pdfGateway = pdfGateway ?? throw new ArgumentNullException(nameof(pdfGateway));
        }

        public void Execute(RenderPdfInput inputTo, IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter)
        {
            var output = _pdfGateway.ConvertToPdf(inputTo);

            if (RenderErrors(output))
            {
                RespondWithErrors(presenter, output);
            }

            RespondWithPdf(inputTo, presenter, output);
        }

        private void RespondWithPdf(RenderPdfInput inputTo, IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter,
            RenderedDocummentOutput output)
        {
            presenter.Respond(new PdfFileOutput(inputTo.FileName, output.FetchDocumentAsByteArray()));
        }

        private void RespondWithErrors(IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter, RenderedDocummentOutput output)
        {
            var errorOutput = new ErrorOutput();
            errorOutput.AddErrors(output.ErrorMessages);
            presenter.Respond(errorOutput);
        }

        private bool RenderErrors(RenderedDocummentOutput output)
        {
            return output.HasErrors();
        }
    }
}