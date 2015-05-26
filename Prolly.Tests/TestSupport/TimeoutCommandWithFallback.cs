using Prolly.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Tests.TestSupport
{
    class TimeoutCommandWithFallback : ProllyCommand<string>
    {
        public const string ReturnValue = "prolly";
        public const string FallbackValue = "fallback";

        private TimeSpan _timeToSleep;

        public TimeoutCommandWithFallback(TimeSpan timeToSleep)
            : base(CommandGroupKey.Factory.Resolve("TimeoutCommandWithFallback"))
        {
            _timeToSleep = timeToSleep;
        }

        protected override string Run()
        {
            System.Threading.Thread.Sleep(_timeToSleep);
            return ReturnValue;
        }

        protected override string Fallback()
        {
            return FallbackValue;
        }
    }
}
