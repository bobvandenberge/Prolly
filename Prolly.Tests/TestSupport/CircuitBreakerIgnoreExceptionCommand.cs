using Prolly.Commands;
using Prolly.Exceptions;
using Prolly.Patterns.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Tests.TestSupport
{
    class CircuitBreakerIgnoreExceptionCommand : ProllyCommand<string>
    {

        public CircuitBreakerIgnoreExceptionCommand()
            : base("ExceptionCommand", new SimpleTimeout(TimeSpan.FromSeconds(30)))
        {
        }

        protected override string Run()
        {
            throw new CircuitBreakerIgnoreException();
        }
    }
}
