using System.IO;
using Mustache.Reports.Data.ReportRendering;
using System;
using Microsoft.Extensions.Options;
using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Options;
using Mustache.Reports.Boundary.Report;
using Mustache.Reports.Boundary.Report.Excel;
using Mustache.Reports.Boundary.Report.Word;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Presenters;
using StoneAge.Synchronous.Process.Runner;
using StoneAge.Synchronous.Process.Runner.PipeLineTask;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Data
{
    public class ReportGateway : IReportGateway
    {
        private readonly MustacheReportOptions _options;

        public ReportGateway(IOptions<MustacheReportOptions> options)
        {
            _options = options.Value;
        }

        public RenderedDocumentOutput CreateWordReport(RenderWordInput input)
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

            var result = CreateReport(factoryArguments, renderFactory);
            result.ContentType = ContentTypes.Excel;
            return result;
        }

        public RenderedDocumentOutput CreateExcelReport(RenderExcelInput input)
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

            var result =  CreateReport(factoryArguments, renderFactory);
            result.ContentType = ContentTypes.Excel;
            return result;
        }

        private RenderedDocumentOutput CreateReport(ReportFactoryArguments arguements,
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

                var presenter = new PropertyPresenter<string, ErrorOutput>();
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
        
        private bool RenderingErrors(PropertyPresenter<string, ErrorOutput> presenter)
        {
            return presenter.IsErrorResponse();
        }
        
        private RenderedDocumentOutput ReturnInvalidReportTemplatePathError(string templatePath)
        {
            var result = new RenderedDocumentOutput();
            result.ErrorMessages.Add($"Invalid Report Template [{templatePath}]");
            return result;
        }

        private bool InvalidReportTemplatePath(string reportTemplatePath)
        {
            return !File.Exists(reportTemplatePath);
        }

        private RenderedDocumentOutput ReturnRenderedReport(PropertyPresenter<string, ErrorOutput> presenter)
        {
            var base64Report = presenter.SuccessContent.TrimEnd('\r', '\n');
            return new RenderedDocumentOutput {Base64String = base64Report};
        }

        private RenderedDocumentOutput ReturnErrors(PropertyPresenter<string, ErrorOutput> presenter)
        {
            var result = new RenderedDocumentOutput();
            result.ErrorMessages.AddRange(presenter.ErrorContent.Errors);
            return result;
        }

        private void RenderReport(ReportGenerationArguments arguments,
                                  Func<string, ReportGenerationArguments, NodePipeLineTask> taskFactory, 
                                  IRespondWithSuccessOrError<string, ErrorOutput> presenter)
        {
            var nodeAppPath = Fetch_NodeApp_Path(_options.NodeApp);

            var task = taskFactory.Invoke(nodeAppPath, arguments);

            var executor = new SynchronousAction(task, new ProcessFactory());
            executor.Execute(presenter);
        }

        private string Fetch_NodeApp_Path(NodeAppOptions optionsNodeApp)
        {
            var prefix = string.Empty;
            if (_options.Template.IsRelative)
            {
                prefix = Directory.GetCurrentDirectory();
            }

            return Path.Combine(prefix, _options.NodeApp.RootDirectory, "cmdLineRender.js");
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
