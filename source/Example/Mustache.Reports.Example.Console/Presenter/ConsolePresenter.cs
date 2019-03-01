using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Example.Console.Presenter
{
    public class ConsolePresenter : IConsolePresenter
    {
        private readonly ILogger<ConsolePresenter> _logger;

        private IFileOutput _file;
        private ErrorOutput _errors;

        public ConsolePresenter(ILogger<ConsolePresenter> logger)
        {
            _logger = logger;
        }

        public void Respond(IFileOutput output)
        {
            _file = output;
        }

        public void Respond(ErrorOutput output)
        {
            _errors = output;
        }

        public void Render(string outputDirectory)
        {
            if (_errors != null)
            {
                Display_Errors();
                return;
            }

            Persist_Report(outputDirectory, _file);

            Display_File_Location(outputDirectory);
        }

        private void Display_File_Location(string outputDirectory)
        {
            _logger.LogInformation($"Report output to directory [ {outputDirectory} ]");
            _logger.LogInformation("");
            _logger.LogInformation("Press enter to exit.");
        }

        private void Display_Errors()
        {
            var joinedErrors = string.Join(", ", (IEnumerable<string>) _errors);
            _logger.LogError("The following errors occured: ");
            _logger.LogError(joinedErrors);
            _logger.LogError("");
            _logger.LogError("Press enter to exit.");
        }

        private void Persist_Report(string reportOutputDirectory, 
                                    IFileOutput successContent)
        {
            Ensure_Directory(reportOutputDirectory);

            var reportPath = Path.Combine(reportOutputDirectory, $"{successContent.FileName}.pdf");
            Remove_Old_Report(reportPath);

            Write_Report(successContent, reportPath);

            return;
        }

        private void Write_Report(IFileOutput successContent, 
                                  string reportPath)
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
