using System;
using System.Collections.Generic;
using NExpect;
using NExpect.Interfaces;
using NExpect.MatcherLogic;
using PeanutButter.Utils;
using TddBuddy.CleanArchitecture.Domain.Messages;
using static NExpect.Expectations;

namespace Mustache.Reports.Domain.Tests.Rendering
{
    public static class ErrorOutputExpectations
    {
        public static IMore<T> ErrorsFor<T>(this IHave<T> have, Exception expected) 
            where T: ErrorOutput
        {
            return have.Compose(errorOutput =>
            {
                Expect(errorOutput).Not.To.Be.Null("ErrorOutput was null");
                Expect(errorOutput.Errors).To.Be.Equivalent.To(expected.AllMessages());    
            });
        }

        public static IMore<T> NoErrors<T>(this IHave<T> have) 
            where T: ErrorOutput
        {
            return have.Compose(errorOutput =>
            {
                var errors = errorOutput?.Errors?.EmptyIfNull() ?? new List<string>();
                Expect(errors).To.Be.Empty();
            });
        }
    }
}