using System.IO;
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

        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(typeof(ErrorOutputMessage), 422)]
        //[HttpPost("create")]
        //public void Create([FromBody] RenderReportInputMessage inputMessage)
        //{
        //    var presenter = new DownloadFilePresenter();
        //    _usecase.Execute(inputMessage, presenter);
        //    presenter.Render();
        //}

        [ProducesResponseType(typeof(File), 200)]
        [Produces("application/pdf")]
        //[ProducesResponseType(typeof(ErrorOutputMessage), 422)]
        [HttpGet("create")]
        public void Create()
        {
            var pwd = Directory.GetCurrentDirectory();
            var jsonData = File.ReadAllText("ExampleData\\WithImagesSampleData.json");

            var inputMessage = new RenderReportInputMessage
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport",
                JsonModel = jsonData
            };
            var presenter = new DownloadFilePresenter();
            _usecase.Execute(inputMessage, presenter);
            presenter.Render();
        }
    }
}
