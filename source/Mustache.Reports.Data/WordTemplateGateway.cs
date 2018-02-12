using System.IO;
using Microsoft.Extensions.Configuration;
using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Report;
using Mustache.Reports.Data.ReportRendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using TddBuddy.Synchronous.Process.Runner;

namespace Mustache.Reports.Data
{
    public class WordTemplateGateway : IWordTemplaterGateway
    {
        private readonly string _templateLocation;
        private readonly string _nodeAppLocation;

        public WordTemplateGateway(IConfiguration configuration)
        {
            _templateLocation = configuration["Reporting:RelativeReportTemplateLocation"];
            _nodeAppLocation = configuration["Reporting:RelativeToExampleNodeAppLocation"];
        }

        public RenderedDocummentOutput CreateReport(RenderWordInput input)
        {
            using (var renderDirectory = GetWorkspace())
            {
                var reportJsonPath = PersitReportData(input, renderDirectory.TmpPath);
                var reportTemplatePath = FetchReportTemplatePath(input.TemplateName);

                if (InvalidReportTemplatePath(reportTemplatePath))
                {
                    return ReturnInvalidReportTemplatePathError(input);
                }

                var presenter = new PropertyPresenter<string, ErrorOutputMessage>();
                RenderReport(reportTemplatePath, reportJsonPath, presenter);

                return RenderingErrors(presenter) ? ReturnErrors(presenter) : ReturnRenderedReport(presenter);
            }
        }

        private string PersitReportData(RenderWordInput input, string renderDirectory)
        {
            var reportJsonPath = Path.Combine(renderDirectory, "reportData.json");
            WriteTo(reportJsonPath, input.JsonModel.ToString());
            return reportJsonPath;
        }
        
        private bool RenderingErrors(PropertyPresenter<string, ErrorOutputMessage> presenter)
        {
            return presenter.IsErrorResponse();
        }
        
        private RenderedDocummentOutput ReturnInvalidReportTemplatePathError(RenderWordInput input)
        {
            var result = new RenderedDocummentOutput();
            result.ErrorMessages.Add($"Invalid Report Type [{input.TemplateName}]");
            return result;
        }

        private bool InvalidReportTemplatePath(string reportTemplatePath)
        {
            return string.IsNullOrEmpty(reportTemplatePath);
        }

        private RenderedDocummentOutput ReturnRenderedReport(PropertyPresenter<string, ErrorOutputMessage> presenter)
        {
            var base64Report = presenter.SuccessContent.TrimEnd('\r', '\n');
            return new RenderedDocummentOutput {Base64String = base64Report};
        }

        private RenderedDocummentOutput ReturnErrors(PropertyPresenter<string, ErrorOutputMessage> presenter)
        {
            var result = new RenderedDocummentOutput();
            result.ErrorMessages.AddRange(presenter.ErrorContent.Errors);
            return result;
        }

        private void RenderReport(string reportTemplatePath, string reportJsonPath, IRespondWithSuccessOrError<string, ErrorOutputMessage> presenter)
        {
            var nodeAppPath = Path.Combine(_nodeAppLocation, "cmdLineRender.js");

            var executor = new SynchronousAction(new ReportRenderTask(nodeAppPath, reportTemplatePath, reportJsonPath), new ProcessFactory());
            executor.Execute(presenter);
        }

        private void WriteTo(string filePath, string data)
        {
            File.WriteAllText(filePath, data);
        }

        private DisposableWorkSpace GetWorkspace()
        {
            return new DisposableWorkSpace();
        }

        private string FetchReportTemplatePath(string reportType)
        {
            var path = Path.Combine(_templateLocation, $"{reportType.ToLower()}.docx");
            var pathExist = File.Exists(path);
            return pathExist ? path : string.Empty;
        }
    }
}
