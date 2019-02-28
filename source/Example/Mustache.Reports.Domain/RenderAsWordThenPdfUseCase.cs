using System;
using System.IO;
using Mustache.Reports.Boundary.Pdf;
using Mustache.Reports.Boundary.Report.Word;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;
using StoneAge.CleanArchitecture.Domain.Presenters;

namespace Mustache.Reports.Domain
{
    public class RenderAsWordThenPdfUseCase : IRenderAsWordThenPdfUseCase
    {
        private readonly IRenderWordUseCase _wordUsecase;
        private readonly IRenderDocxToPdfUseCase _pdfUsecase;

        public RenderAsWordThenPdfUseCase(IRenderWordUseCase wordUsecase, 
                                          IRenderDocxToPdfUseCase pdfUsecase)
        {
            _wordUsecase = wordUsecase;
            _pdfUsecase = pdfUsecase;
        }

        public void Execute(RenderWordInput inputTo, 
            IRespondWithSuccessOrError<IFileOutput, ErrorOutput> presenter)
        {
            var wordPresenter = new PropertyPresenter<IFileOutput, ErrorOutput>();

            _wordUsecase.Execute(inputTo, wordPresenter);

            if (wordPresenter.IsErrorResponse())
            {
                presenter.Respond(wordPresenter.ErrorContent);
                return;
            }

            var pdfInput = Create_Pdf_Input(wordPresenter);
            _pdfUsecase.Execute(pdfInput, presenter);
        }

        private static RenderPdfInput Create_Pdf_Input(PropertyPresenter<IFileOutput, ErrorOutput> docxPresenter)
        {
            var input = new RenderPdfInput();
            using (var stream = docxPresenter.SuccessContent.GetStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    var reportBytes = memoryStream.ToArray();
                    input.Base64DocxReport = Convert.ToBase64String(reportBytes);
                }
            }
            return input;
        }
    }
}