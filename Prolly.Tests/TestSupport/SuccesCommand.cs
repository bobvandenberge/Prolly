using Prolly.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Tests.TestSupport
{
    class SuccesCommand : ProllyCommand<string>
    {
        public const string ReturnValue = "prolly";

        public SuccesCommand()
            : base(CommandGroupKey.Factory.Resolve(String.Empty))
        { }

        protected override string Run()
        {
            return ReturnValue;
        }
    }
}
