using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Boundary.Rendering.Word
{
    public interface IRenderWordReportUseCase
    {
        void Execute(RenderReportInput input, IRespondWithSuccessOrError<IWordFileOutput, ErrorOutput> presenter);
    }
}