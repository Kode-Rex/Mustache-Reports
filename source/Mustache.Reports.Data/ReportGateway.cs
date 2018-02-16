using System.IO;
using Microsoft.Extensions.Configuration;
using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Report.Word;
using Mustache.Reports.Data.ReportRendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using TddBuddy.Synchronous.Process.Runner;
using Mustache.Reports.Boundry.Report.Excel;
using Mustache.Reports.Boundry.Report;

namespace Mustache.Reports.Data
{
    public class ReportGateway : IReportGateway
    {
        private readonly string _templateLocation;
        private readonly string _nodeAppLocation;

        public ReportGateway(IConfiguration configuration)
        {
            _templateLocation = configuration["Reporting:RelativeReportTemplateLocation"];
            _nodeAppLocation = configuration["Reporting:RelativeNodeAppLocation"];
        }

        public RenderedDocummentOutput CreateWordReport(RenderWordInput input)
        {
            return CreateReport(input.JsonModel.ToString(), input.TemplateName, "docx");
        }

        public RenderedDocummentOutput CreateExcelReport(RenderExcelInput input)
        {
            return CreateReport(input.JsonModel.ToString(), input.TemplateName, "xlsx");
        }

        private RenderedDocummentOutput CreateReport(string jsonModel, string templateName, string extension)
        {
            using (var renderDirectory = GetWorkspace())
            {
                var reportJsonPath = PersitReportData(jsonModel, renderDirectory.TmpPath);
                var reportTemplatePath = FetchReportTemplatePath(templateName, extension);

                if (InvalidReportTemplatePath(reportTemplatePath))
                {
                    return ReturnInvalidReportTemplatePathError(templateName);
                }

                var presenter = new PropertyPresenter<string, ErrorOutputMessage>();
                RenderReport(reportTemplatePath, reportJsonPath, presenter);

                return RenderingErrors(presenter) ? ReturnErrors(presenter) : ReturnRenderedReport(presenter);
            }
        }

        private string PersitReportData(string reportData, string renderDirectory)
        {
            var reportJsonPath = Path.Combine(renderDirectory, "reportData.json");
            WriteTo(reportJsonPath, reportData);
            return reportJsonPath;
        }
        
        private bool RenderingErrors(PropertyPresenter<string, ErrorOutputMessage> presenter)
        {
            return presenter.IsErrorResponse();
        }
        
        private RenderedDocummentOutput ReturnInvalidReportTemplatePathError(string templateName)
        {
            var result = new RenderedDocummentOutput();
            result.ErrorMessages.Add($"Invalid Report Type [{templateName}]");
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

            var executor = new SynchronousAction(new WordRender(nodeAppPath, reportTemplatePath, reportJsonPath), new ProcessFactory());
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

        private string FetchReportTemplatePath(string reportType, string extension)
        {
            var path = Path.Combine(_templateLocation, $"{reportType.ToLower()}.{extension}");
            var pathExist = File.Exists(path);
            return pathExist ? path : string.Empty;
        }
    }
}
