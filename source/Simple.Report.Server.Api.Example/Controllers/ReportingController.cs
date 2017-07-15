using Microsoft.AspNetCore.Mvc;
using Simple.Report.Server.Domain.Messages;
using Simple.Report.Server.Domain.Messages.Input;
using Simple.Report.Server.Domain.UseCases;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Presenters;

namespace Simple.Report.Server.Api.Example.Controllers
{
    [Route("api/v1/reporting")]
    public class ReportingController
    {
        private readonly IRenderReportUseCase _usecase;

        public ReportingController(IRenderReportUseCase usecase)
        {
            _usecase = usecase;
        }

        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorOutputMessage), 422)]
        [HttpPost("create")]
        public void Create([FromBody] RenderReportInputMessage inputMessage)
        {
            var presenter = new DownloadFilePresenter();
            _usecase.Execute(inputMessage, presenter);
            presenter.Render();
        }
    }
}
