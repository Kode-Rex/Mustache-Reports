using System;
using System.Collections.Generic;
using System.IO;
using Simple.Report.Server.Boundry.ReportRendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using TddBuddy.Synchronous.Process.Runner;

namespace Simple.Report.Server.Data.ReportRendering
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

        public RenderedReportOutputMessage CreatePdfReport(RenderReportInputMessage inputMessage)
        {
            var reportJsonPath = WriteReportJson(inputMessage.JsonModel.ToString());
            var reportTemplatePath = FetchReportTemplatePath(inputMessage.TemplateName);

            if (InvalidReportTemplatePath(reportTemplatePath))
            {
                return ReturnInvalidReportTemplatePathError(inputMessage);
            }

            var presenter = RenderReport(reportTemplatePath, reportJsonPath);

            if (presenter.IsErrorResponse())
            {
                return ReturnRenderErrors(presenter);
            }

            return ReturnRenderedReport(presenter);
        }

        private RenderedReportOutputMessage ReturnInvalidReportTemplatePathError(RenderReportInputMessage inputMessage)
        {
            return new RenderedReportOutputMessage
            {
                ErrorMessages = $"Invalid Report Type [{inputMessage.TemplateName}]"
            };
        }

        private bool InvalidReportTemplatePath(string reportTemplatePath)
        {
            return string.IsNullOrEmpty(reportTemplatePath);
        }

        private PropertyPresenter<string, ErrorOutputMessage> RenderReport(string reportTemplatePath, string reportJsonPath)
        {
            var presenter = new PropertyPresenter<string, ErrorOutputMessage>();
            RenderReport(reportTemplatePath, reportJsonPath, presenter);
            return presenter;
        }

        private RenderedReportOutputMessage ReturnRenderedReport(PropertyPresenter<string, ErrorOutputMessage> presenter)
        {
            var base64Report = presenter.SuccessContent.TrimEnd('\r', '\n');
            return new RenderedReportOutputMessage {ReportAsBase64String = base64Report};
        }

        private RenderedReportOutputMessage ReturnRenderErrors(PropertyPresenter<string, ErrorOutputMessage> presenter)
        {
            var errorsEnumerable = presenter.ErrorContent.Errors.ToArray() as IEnumerable<string>;
            var errorString = string.Join(", ", errorsEnumerable);
            return new RenderedReportOutputMessage {ErrorMessages = errorString};
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
