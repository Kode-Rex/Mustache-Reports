using Simple.Report.Server.Domain.Messages;

namespace Simple.Report.Server.Domain.Repositories
{
    public interface IReportRepository
    {
        RenderedReportMessage CreateReport(CreateReportMessage message);
    }
}
