using System;
using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Pdf;
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

        public void Execute(RenderPdfInput inputTo, 
                            IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter)
        {
            var output = _pdfGateway.ConvertToPdf(inputTo);

            if (Render_Errors(output))
            {
                Respond_With_Errors(presenter, output);
                return;
            }

            Respond_With_Pdf(inputTo, presenter, output);
        }

        private void Respond_With_Pdf(RenderPdfInput inputTo, 
                                    IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter,
                                    RenderedDocumentOutput output)
        {
            presenter.Respond(new PdfFileOutput(inputTo.FileName, output.FetchDocumentAsByteArray()));
        }

        private void Respond_With_Errors(IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter, 
                                       RenderedDocumentOutput output)
        {
            var errorOutput = new ErrorOutput();
            errorOutput.AddErrors(output.ErrorMessages);
            presenter.Respond(errorOutput);
        }

        private bool Render_Errors(RenderedDocumentOutput output)
        {
            return output.HasErrors();
        }
    }
}