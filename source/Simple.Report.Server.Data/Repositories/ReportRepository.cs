using System;
using System.IO;
using Simple.Report.Server.Data.Task;
using Simple.Report.Server.Domain.Messages;
using Simple.Report.Server.Domain.Repositories;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using TddBuddy.Synchronous.Process.Runner;

namespace Simple.Report.Server.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _templateLocation;
        private readonly string _nodeAppLocation;

        public ReportRepository(string templateLocation, string nodeAppLocation)
        {
            _templateLocation = templateLocation;
            _nodeAppLocation = nodeAppLocation;
        }

        public RenderedReportMessage CreateReport(CreateReportMessage message)
        {
            var reportJsonPath = WriteReportJson(message.JsonModel);
            var reportTemplatePath = FetchReportTemplatePath(message.TemplateName);

            if (string.IsNullOrEmpty(reportTemplatePath))
            {
                throw new Exception($"Invalid Report Type [{message.TemplateName}]");
            }

            var presenter = new PropertyPresenter<string, ErrorOutputMessage>();
            RenderReport(reportTemplatePath, reportJsonPath, presenter);

            if (presenter.IsErrorResponse())
            {
                var errors =  string.Join(", ", presenter.ErrorContent.Errors.ToArray());
                return new RenderedReportMessage {ErrorMessages = errors};
            }

            var base64Report = presenter.SuccessContent.TrimEnd('\r', '\n');
            return new RenderedReportMessage { ReportAsBase64String = base64Report };
        }

        private void RenderReport(string reportTemplatePath, string reportJsonPath, IRespondWithSuccessOrError<string, ErrorOutputMessage> presenter)
        {
            var nodeAppPath = Path.Combine(_nodeAppLocation, "cmdLineRender.js");

            var executor = new SynchronousAction(new ReportRenderTask(nodeAppPath, reportTemplatePath, reportJsonPath), new ProcessFactory());
            executor.Execute(presenter);
        }

        private string WriteReportJson(string reportData)
        {
            var reportJsonPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            File.WriteAllText(reportJsonPath, reportData);
            return reportJsonPath;
        }

        private string FetchReportTemplatePath(string reportType)
        {
            var path = Path.Combine(_templateLocation, $"{reportType.ToLower()}.docx");
            var pathExist = File.Exists(path);
            return pathExist ? path : string.Empty;
        }
    }
}
