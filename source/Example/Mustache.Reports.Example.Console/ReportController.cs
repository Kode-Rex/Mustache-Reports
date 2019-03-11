using System.IO;
using System.Reflection;
using Mustache.Reports.Boundary.Report.Word;
using Mustache.Reports.Domain.Pdf;
using Mustache.Reports.Example.Console.Domain;

namespace Mustache.Reports.Example.Console
{
    public class ReportController
    {
        private readonly IRenderAsWordThenPdfUseCase _usecase;
        private readonly IConsolePresenter _presenter;

        public ReportController(IRenderAsWordThenPdfUseCase usecase, 
                                IConsolePresenter presenter)
        {
            _usecase = usecase;
            _presenter = presenter;
        }

        public void Run(string reportOutputDirectory, 
                        string reportDataFilePath)
        {
            Render_Report_With_Images(reportOutputDirectory, reportDataFilePath);
        }

        private void Render_Report_With_Images(string reportOutputDirectory, 
                                               string reportDataFilePath)
        {
            var jsonData = Read_Report_Data(reportDataFilePath);
            var inputMessage = Create_Word_Input_Message(jsonData);

            _usecase.Execute(inputMessage, _presenter);
    
            _presenter.Render(reportOutputDirectory);
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
    }
}
