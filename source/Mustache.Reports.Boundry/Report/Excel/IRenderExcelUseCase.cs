using StoneAge.CleanArchitecture.Domain;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Boundary.Report.Excel
{
    public interface IRenderExcelUseCase : IUseCase<RenderExcelInput, IFileOutput>
    {
    }
}
