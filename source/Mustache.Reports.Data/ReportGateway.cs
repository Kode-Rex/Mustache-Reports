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
using System;
using TddBuddy.Synchronous.Process.Runner.PipeLineTask;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace Mustache.Reports.Data
{
    public class ReportGateway : IReportGateway
    {
        private readonly MustacheReportOptions _options;

        public ReportGateway(IOptions<MustacheReportOptions> options)
        {
            _options = options.Value;
        }

        public RenderedDocummentOutput CreateWordReport(RenderWordInput input)
        {
            Func<string, ReportGenerationArguments, NodePipeLineTask> renderFactory = (nodeAppPath, arguements) =>
            {
                return new WordRender(nodeAppPath, arguements.TemplatePath, arguements.JsonPath);
            };

            var factoryArguments = new ReportFactoryArguments
            {
                ReportJson = input.JsonModel.ToString(),
                TemplateName = input.TemplateName,
                Extension = "docx"
            };

            return CreateReport(factoryArguments, renderFactory);
        }

        public RenderedDocummentOutput CreateExcelReport(RenderExcelInput input)
        {
            Func<string, ReportGenerationArguments, NodePipeLineTask> renderFactory = (nodeAppPath, arguments) =>
            {
                return new ExcelRender(nodeAppPath, arguments.TemplatePath, arguments.JsonPath, arguments.SheetNumber);
            };

            var factoryArguments = new ReportFactoryArguments
            {
                ReportJson = input.JsonModel.ToString(),
                TemplateName = input.TemplateName,
                Extension = "xlsx",
                SheetNumber = input.SheetNumber
            };

            return CreateReport(factoryArguments, renderFactory);
        }

        private RenderedDocummentOutput CreateReport(ReportFactoryArguments arguements,
                                                    Func<string, ReportGenerationArguments, NodePipeLineTask> taskFactory)
        {
            using (var renderDirectory = GetWorkspace())
            {
                var reportJsonPath = PersitReportData(arguements.ReportJson, renderDirectory.TmpPath);
                var reportTemplatePath = FetchReportTemplatePath(arguements.TemplateName, arguements.Extension);

                if (InvalidReportTemplatePath(reportTemplatePath))
                {
                    return ReturnInvalidReportTemplatePathError(reportTemplatePath);
                }

                var presenter = new PropertyPresenter<string, ErrorOutputMessage>();
                var reportArguments = new ReportGenerationArguments
                {
                    TemplatePath = reportTemplatePath,
                    JsonPath = reportJsonPath,
                    SheetNumber = arguements.SheetNumber
                };
                RenderReport(reportArguments, taskFactory, presenter);

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
        
        private RenderedDocummentOutput ReturnInvalidReportTemplatePathError(string templatePath)
        {
            var result = new RenderedDocummentOutput();
            result.ErrorMessages.Add($"Invalid Report Template [{templatePath}]");
            return result;
        }

        private bool InvalidReportTemplatePath(string reportTemplatePath)
        {
            return !File.Exists(reportTemplatePath);
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

        private void RenderReport(ReportGenerationArguments arguments,
                                  Func<string, ReportGenerationArguments, NodePipeLineTask> taskFactory, 
                                  IRespondWithSuccessOrError<string, ErrorOutputMessage> presenter)
        {
            var nodeAppPath = Path.Combine(_options.NodeApp.RootDirectory, "cmdLineRender.js");
            var task = taskFactory.Invoke(nodeAppPath, arguments);

            var executor = new SynchronousAction(task, new ProcessFactory());
            executor.Execute(presenter);
        }

        private void RenderReport(string reportTemplatePath,
                                 string reportJsonPath,
                                 Func<string, string, string, NodePipeLineTask> taskFactory,
                                 IRespondWithSuccessOrError<string, ErrorOutputMessage> presenter)
        {
            var nodeAppPath = Path.Combine(_options.NodeApp.RootDirectory, "cmdLineRender.js");
            var task = taskFactory.Invoke(nodeAppPath, reportTemplatePath, reportJsonPath);

            var executor = new SynchronousAction(task, new ProcessFactory());
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

        private string FetchReportTemplatePath(string reportName, string extension)
        {
            var prefix = string.Empty;
            if (_options.Template.IsRelative)
            {
                prefix = Directory.GetCurrentDirectory();
            }
            return Path.Combine(prefix, _options.Template.RootDirectory, $"{reportName}.{extension}");
        }
    }
}
