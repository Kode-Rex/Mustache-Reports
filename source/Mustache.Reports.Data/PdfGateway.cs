using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Pdf;
using Mustache.Reports.Data.PdfRendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using TddBuddy.Synchronous.Process.Runner;

namespace Mustache.Reports.Data
{
    public class PdfGateway : IPdfGateway
    {
        private readonly string _libreOffice;

        public PdfGateway(IConfiguration configuration)
        {
            _libreOffice = configuration["Reporting:LibreOfficeLocation"];
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

        private RenderedDocummentOutput ReturnErrors(PropertyPresenter<string, ErrorOutputMessage> presenter)
        {
            var result = new RenderedDocummentOutput();
            result.ErrorMessages.AddRange(presenter.ErrorContent.Errors);
            return result;
        }

        private void WriteTo(string filePath, string data)
        {
            var decodedData = Convert.FromBase64String(data);
            File.WriteAllBytes(filePath, decodedData);
        }

        private DisposableWorkSpace GetWorkspace()
        {
            return new DisposableWorkSpace();
        }
    }
}
