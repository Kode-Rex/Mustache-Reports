using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Simple.Report.Server.Boundry;
using Simple.Report.Server.Boundry.Rendering;
using Simple.Report.Server.Boundry.Rendering.Pdf;
using Simple.Report.Server.Boundry.Rendering.Report;
using Simple.Report.Server.Data.PdfRendering;
using Simple.Report.Server.Data.ReportRendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using TddBuddy.Synchronous.Process.Runner;

namespace Simple.Report.Server.Data
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _templateLocation;
        private readonly string _nodeAppLocation;
        private readonly string _libreOffice;

        public ReportRepository(IConfiguration configuration)
        {
            _templateLocation = configuration["Reporting:RelativeReportTemplateLocation"];
            _nodeAppLocation = configuration["Reporting:RelativeToExampleNodeAppLocation"];
            _libreOffice = configuration["Reporting:LibreOfficeLocation"];
        }

        public RenderedDocummentOutput CreateReport(RenderReportInput input)
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

        public RenderedDocummentOutput ConvertToPdf(RenderPdfInput inputMessage)
        {
            using (var renderDirectory = GetWorkspace())
            {
                var pdfPresenter = new PropertyPresenter<string, ErrorOutputMessage>();

                var reportPath = PersistDocxFile(inputMessage, renderDirectory);

                CovertToPdf(reportPath, renderDirectory, pdfPresenter);

                return RenderingErrors(pdfPresenter) ? ReturnErrors(pdfPresenter) : ReturnPdf(reportPath);
            }
        }

        private void CovertToPdf(string reportPath, DisposableWorkSpace renderDirectory, PropertyPresenter<string, ErrorOutputMessage> pdfPresenter)
        {
            var executor = new SynchronousAction(new DocxToPdfTask(_libreOffice, reportPath, renderDirectory.TmpPath),
                new ProcessFactory());
            executor.Execute(pdfPresenter);
        }

        private string PersitReportData(RenderReportInput input, string renderDirectory)
        {
            var reportJsonPath = Path.Combine(renderDirectory, "reportData.json");
            WriteTo(reportJsonPath, input.JsonModel.ToString());
            return reportJsonPath;
        }
        
        private bool RenderingErrors(PropertyPresenter<string, ErrorOutputMessage> presenter)
        {
            return presenter.IsErrorResponse();
        }

        private RenderedDocummentOutput ReturnPdf(string reportPath)
        {
            var pdfPath = reportPath.Replace(".docx", ".pdf");
            var result = new RenderedDocummentOutput
            {
                Base64String = Convert.ToBase64String(File.ReadAllBytes(pdfPath))
            };
            return result;
        }

        private string PersistDocxFile(RenderPdfInput inputMessage, DisposableWorkSpace renderDirectory)
        {
            var reportPath = Path.Combine(renderDirectory.TmpPath, "report.docx");
            WriteTo(reportPath, inputMessage.Base64DocxReport);
            return reportPath;
        }

        private RenderedDocummentOutput ReturnInvalidReportTemplatePathError(RenderReportInput input)
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
