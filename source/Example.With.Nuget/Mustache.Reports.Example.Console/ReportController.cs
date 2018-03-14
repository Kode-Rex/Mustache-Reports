using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Mustache.Reports.Boundry.Pdf;
using Mustache.Reports.Boundry.Report.Word;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;

namespace Mustache.Reports.Example.Console
{
    public class ReportController
    {
        private readonly IRenderWordUseCase _wordUseCase;
        private readonly IRenderDocxToPdfUseCase _pdfUseCase;
        private readonly ILogger _logger;

        public ReportController(IRenderWordUseCase wordUseCase, IRenderDocxToPdfUseCase pdfUseCase, ILoggerFactory logFactory)
        {
            _wordUseCase = wordUseCase;
            _pdfUseCase = pdfUseCase;
            _logger = logFactory.CreateLogger<ReportController>();
        }

        public void Run( string reportOutputDirectory, string reportDataFilePath)
        {
            RenderReportWithImages(reportOutputDirectory, reportDataFilePath);
        }

        private void RenderReportWithImages(string reportOuputDirectory, string reportDataFilePath)
        {
            var jsonData = File.ReadAllText(reportDataFilePath);

            var inputMessage = new RenderWordInput
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport",
                JsonModel = jsonData
            };

            var docxPresenter = new PropertyPresenter<IFileOutput, ErrorOutputMessage>();
            _wordUseCase.Execute(inputMessage, docxPresenter);

            if (docxPresenter.IsErrorResponse())
            {
                WriteErrorsToConsole(docxPresenter);
                return;
            }

            var input = CreateRenderPdfInput(docxPresenter);

            var pdfPresenter = new PropertyPresenter<IFileOutput, ErrorOutputMessage>();
            _pdfUseCase.Execute(input,pdfPresenter);

            if (pdfPresenter.IsErrorResponse())
            {
                WriteErrorsToConsole(pdfPresenter);
                return;
            }

            var pdfPath = PersistReport(reportOuputDirectory, pdfPresenter.SuccessContent);
            
            _logger.LogInformation($"Report output to directory [ {pdfPath} ]");
            _logger.LogInformation("");
            _logger.LogInformation("Press enter to exit.");
        }

        private static RenderPdfInput CreateRenderPdfInput(PropertyPresenter<IFileOutput, ErrorOutputMessage> docxPresenter)
        {
            var input = new RenderPdfInput();
            using (var stream = docxPresenter.SuccessContent.GetStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    var reportBytes = memoryStream.ToArray();
                    input.Base64DocxReport = Convert.ToBase64String(reportBytes);
                }
            }
            return input;
        }

        private void WriteErrorsToConsole<T>(PropertyPresenter<T, ErrorOutputMessage> presenter)
        {
            _logger.LogError("The following errors occured: ");
            _logger.LogError(string.Join(", ", (IEnumerable<string>)presenter.ErrorContent.Errors));
            _logger.LogError("");
            _logger.LogError("Press enter to exit.");
        }

        private string PersistReport(string reportOuputDirectory, IFileOutput successContent)
        {
            EnsureDirectory(reportOuputDirectory);

            var reportPath = Path.Combine(reportOuputDirectory, $"{successContent.FileName}.pdf");
            RemoveOldReport(reportPath);

            WriteReport(successContent, reportPath);

            return reportPath;
        }

        private void WriteReport(IFileOutput successContent, string reportPath)
        {
            using (var stream = successContent.GetStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    var reportBytes = memoryStream.ToArray();
                    File.WriteAllBytes(reportPath, reportBytes);
                }
            }
        }

        private void RemoveOldReport(string reportPath)
        {
            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }
        }

        private void EnsureDirectory(string reportOuputDirectory)
        {
            if (!Directory.Exists(reportOuputDirectory))
            {
                Directory.CreateDirectory(reportOuputDirectory);
            }
        }
    }
}
