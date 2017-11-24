using TddBuddy.CleanArchitecture.Domain.Messages;

namespace Simple.Report.Server.Boundry.ReportRendering
{
    public class WordFileOutputMessage : InMemoryFileOutputMessage
    {
        private const string _contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public WordFileOutputMessage(string fileName, byte[] fileData) : base(fileName, fileData, _contentType)
        {
        }
    }
}
