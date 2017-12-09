using Mustache.Reports.Boundary.Rendering;
using Mustache.Reports.Domain.TestData;
using NExpect;
using NExpect.Interfaces;
using NExpect.MatcherLogic;
using static NExpect.Expectations;

namespace Mustache.Reports.Domain.Tests.Rendering
{
    public static class InputFileExpectations
    {
        public static IMore<T> ContentsEqualTo<T>(this IHave<T> have, string expected) 
            where T: IFileInput
        {
            return have.Compose(inputFile =>
            {
                Expect(inputFile).Not.To.Be.Null("Input file was null");

                using (var stream = inputFile.GetStream())
                {
                    Expect(stream.ToBase64String()).To.Equal(expected);
                }
            });
        }
    }
}