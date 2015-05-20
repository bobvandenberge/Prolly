using Prolly.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Tests.TestSupport
{
    class TimeoutCommand : ProllyCommand<string>
    {
        public const string ReturnValue = "prolly";

        private TimeSpan _timeToSleep;

        public TimeoutCommand(TimeSpan timeToSleep)
            :base("Timeout")
        {
            _timeToSleep = timeToSleep;
        }

        protected override string Run()
        {
            System.Threading.Thread.Sleep(_timeToSleep);
            return ReturnValue;
        }
    }
}
