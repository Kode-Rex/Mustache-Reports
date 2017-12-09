using System;
using PeanutButter.RandomGenerators;

namespace Mustache.Reports.Domain.TestData.Errors
{
    public class ExceptionTestDataBuilder
    {
        private int _numberOfInnerExceptions;
        private string _message;

        public static ExceptionTestDataBuilder Create()
        {
            return new ExceptionTestDataBuilder();
        }

        public ExceptionTestDataBuilder WithNestedInnerExceptions(int howMany)
        {
            _numberOfInnerExceptions = howMany;
            return this;
        }

        public ExceptionTestDataBuilder WithRandomMessage()
        {
            _message = RandomValueGen.GetRandomString();
            return this;
        }

        public Exception Build()
        {
            return new Exception(_message, CreateInnerExceptions());
        }

        private Exception CreateInnerExceptions()
        {
            return _numberOfInnerExceptions <= 0
                ? null
                : Create()
                    .WithRandomMessage()
                    .WithNestedInnerExceptions(_numberOfInnerExceptions - 1)
                    .Build();
        }
    }
}