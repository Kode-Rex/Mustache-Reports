using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Mustache.Reports.Boundary.Pdf;
using Mustache.Reports.Boundary.Report.Word;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;
using StoneAge.CleanArchitecture.Domain.Presenters;

namespace Mustache.Reports.Example.Console
{
    public class ReportController
    {
        private readonly IRenderWordUseCase _wordUseCase;
        private readonly IRenderDocxToPdfUseCase _pdfUseCase;
        private readonly ILogger _logger;

        public ReportController(IRenderWordUseCase wordUseCase, 
                                IRenderDocxToPdfUseCase pdfUseCase, 
                                ILogger<ReportController> logger)
        {
            _wordUseCase = wordUseCase;
            _pdfUseCase = pdfUseCase;
            _logger = logger;
        }

        public void Run(string reportOutputDirectory, 
                        string reportDataFilePath)
        {
            Render_Report_With_Images(reportOutputDirectory, reportDataFilePath);
        }

        private void Render_Report_With_Images(string reportOuputDirectory, 
                                               string reportDataFilePath)
        {
            var jsonData = Read_Report_Data(reportDataFilePath);
            var docxPresenter = new PropertyPresenter<IFileOutput, ErrorOutput>();

            var inputMessage = Create_Word_Input_Message(jsonData);

            _wordUseCase.Execute(inputMessage, docxPresenter);

            if (docxPresenter.IsErrorResponse())
            {
                WriteErrorsToConsole(docxPresenter);
                return;
            }

            Convert_Word_To_Pdf(reportOuputDirectory, docxPresenter);
        }

        private void Convert_Word_To_Pdf(string reportOuputDirectory, 
                                         PropertyPresenter<IFileOutput, ErrorOutput> docxPresenter)
        {
            var input = Create_Pdf_Input(docxPresenter);

            var pdfPresenter = new PropertyPresenter<IFileOutput, ErrorOutput>();
            _pdfUseCase.Execute(input, pdfPresenter);

            if (pdfPresenter.IsErrorResponse())
            {
                WriteErrorsToConsole(pdfPresenter);
                return;
            }

            var pdfPath = Persist_Report(reportOuputDirectory, pdfPresenter.SuccessContent);

            _logger.LogInformation($"Report output to directory [ {pdfPath} ]");
            _logger.LogInformation("");
            _logger.LogInformation("Press enter to exit.");
        }

        private static string Read_Report_Data(string reportDataFilePath)
        {
            var baseLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var readPath = Path.Combine(baseLocation, reportDataFilePath);

            var jsonData = File.ReadAllText(readPath);
            return jsonData;
        }

        private static RenderWordInput Create_Word_Input_Message(string jsonData)
        {
            var inputMessage = new RenderWordInput
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport",
                JsonModel = jsonData
            };
            return inputMessage;
        }

        private static RenderPdfInput Create_Pdf_Input(PropertyPresenter<IFileOutput, ErrorOutput> docxPresenter)
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

        private void WriteErrorsToConsole<T>(PropertyPresenter<T, ErrorOutput> presenter)
        {
            _logger.LogError("The following errors occured: ");
            _logger.LogError(string.Join(", ", (IEnumerable<string>)presenter.ErrorContent.Errors));
            _logger.LogError("");
            _logger.LogError("Press enter to exit.");
        }

        private string Persist_Report(string reportOutputDirectory, IFileOutput successContent)
        {
            Ensure_Directory(reportOutputDirectory);

            var reportPath = Path.Combine(reportOutputDirectory, $"{successContent.FileName}.pdf");
            Remove_Old_Report(reportPath);

            Write_Report(successContent, reportPath);

            return reportPath;
        }

        private void Write_Report(IFileOutput successContent, string reportPath)
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

        private void Remove_Old_Report(string reportPath)
        {
            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }
        }

        private void Ensure_Directory(string reportOutputDirectory)
        {
            if (!Directory.Exists(reportOutputDirectory))
            {
                Directory.CreateDirectory(reportOutputDirectory);
            }
        }
    }
}
