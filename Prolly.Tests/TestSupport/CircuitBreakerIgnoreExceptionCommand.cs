using Prolly.Commands;
using Prolly.Exceptions;
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
            : base(CommandGroupKey.Factory.Resolve("ExceptionCommand"))
        {
        }

        protected override string Run()
        {
            throw new CircuitBreakerIgnoreException();
        }
    }
}
