using Mustache.Reports.Boundary.Rendering;
using Mustache.Reports.Boundary.Rendering.Word;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain.Rendering.Word
{
    public class RenderWordReportUseCase : IRenderWordReportUseCase
    {
        private readonly IWordTemplaterGateway _wordTemplaterGateway;

        public RenderWordReportUseCase(IWordTemplaterGateway wordTemplaterGateway)
        {
            _wordTemplaterGateway = wordTemplaterGateway;
        }

        public void Execute(RenderReportInput input, IRespondWithSuccessOrError<IWordFileOutput, ErrorOutput> presenter)
        {
            presenter.Respond(() =>
                _wordTemplaterGateway.Render(new InMemoryWordInputFile(input.Template), input.Data)
            );
        }
    }
}