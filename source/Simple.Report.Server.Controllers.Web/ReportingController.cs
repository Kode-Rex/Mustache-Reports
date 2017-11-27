using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Report.Server.Boundry.ReportRendering;
using TddBuddy.CleanArchitecture.Presenters;

namespace Simple.Report.Server.Controllers.Web
{
    [Route("api/reporting")]
    public class ReportingController
    {
        private readonly IRenderReportUseCase _usecase;

        public ReportingController(IRenderReportUseCase usecase)
        {
            _usecase = usecase;
        }

        [ProducesResponseType(typeof(File), 200)]
        [Produces("application/pdf")]
        //[ProducesResponseType(typeof(ErrorOutputMessage), 422)]
        [HttpGet("create")]
        public IActionResult Create()
        {
            var jsonData = File.ReadAllText("ReportRendering\\ExampleData\\WithImagesSampleData.json");

            var inputMessage = new RenderReportInputMessage
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport",
                JsonModel = jsonData
            };
            var presenter = new DownloadFilePresenter();
            _usecase.Execute(inputMessage, presenter);
            return presenter.Render();
        }

    }
}
