using System.IO;
using Microsoft.AspNetCore.Mvc;
using Mustache.Reports.Boundry.Report;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Presenters;

namespace Mustache.Reports.Controllers.Web
{
    [Route("api/reporting")]
    public class ReportingController
    {
        private readonly IRenderWordUseCase _usecase;

        public ReportingController(IRenderWordUseCase usecase)
        {
            _usecase = usecase;
        }

        [Produces("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [ProducesResponseType(typeof(ErrorOutputMessage), 422)]
        [HttpGet("create/word")]
        public IActionResult Create()
        {
            var jsonData = File.ReadAllText("ReportRendering\\ExampleData\\WithImagesSampleData.json");

            var inputMessage = new RenderWordInput
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport.docx",
                JsonModel = jsonData
            };
            var presenter = new DownloadFilePresenter();
            _usecase.Execute(inputMessage, presenter); // base64 string ok, just the swagger? Yep, download from url works fine.
            return presenter.Render();
        }

    }
}
