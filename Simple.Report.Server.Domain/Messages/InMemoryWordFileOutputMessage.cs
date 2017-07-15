using TddBuddy.CleanArchitecture.Domain.Messages;

namespace Simple.Report.Server.Domain.Messages
{
    public class InMemoryWordFileOutputMessage : InMemoryFileOutputMessage
    {
        private const string _contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public InMemoryWordFileOutputMessage(string fileName, byte[] fileData) : base(fileName, fileData, _contentType)
        {
        }
    }
}
