using TddBuddy.CleanArchitecture.Domain;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Boundry.Report.Excel
{
    public interface IRenderExcelUseCase : IUseCase<RenderExcelInput, IFileOutput>
    {
    }
}
