namespace Simple.Report.Server.Domain.Messages
{
    public class CreateReportMessage
    {
        public string JsonModel { get; set; }
        public string TemplateName { get; set; }
        public string ReportName { get; set; }

        public CreateReportMessage()
        {
            ReportName = "report.docx";
        }
    }
}
