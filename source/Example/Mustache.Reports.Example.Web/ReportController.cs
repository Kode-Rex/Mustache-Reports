using System.IO;
using Microsoft.AspNetCore.Mvc;
using Mustache.Reports.Boundary.Pdf;
using Mustache.Reports.Boundary.Report.Word;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Presenters;

namespace Mustache.Reports.Example.Web
{
    [Route("api/report")]
    public class ReportController
    {
        private readonly IRenderWordUseCase _wordUsecase;
        private readonly IRenderDocxToPdfUseCase _pdfUsecase;

        public ReportController(IRenderWordUseCase wordUsecase, IRenderDocxToPdfUseCase pdfUsecase)
        {
            _wordUsecase = wordUsecase;
            _pdfUsecase = pdfUsecase;
        }

        [Produces("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [ProducesResponseType(typeof(ErrorOutput), 422)]
        [HttpGet("create/word")]
        public IActionResult Create_Word()
        {
            var jsonData = File.ReadAllText("ExampleData\\WithImagesSampleData.json");

            var inputMessage = new RenderWordInput
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport.docx",
                JsonModel = jsonData
            };
            var presenter = new DownloadFilePresenter();
            _wordUsecase.Execute(inputMessage, presenter);
            return presenter.Render();
        }

        [Produces("application/pdf")]
        [ProducesResponseType(typeof(ErrorOutput), 422)]
        [HttpGet("create/pdf")]
        public IActionResult Create_Pdf()
        {
            var jsonData = File.ReadAllText("ExampleData\\WithImagesSampleData.json");

            var inputMessage = new RenderWordInput
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport.docx",
                JsonModel = jsonData
            };
            var presenter = new DownloadFilePresenter();
            _wordUsecase.Execute(inputMessage, presenter);
            return presenter.Render();
        }
    }
}
