using Prolly.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Commands
{
    public class CommandGroup
    {
        public string Name { get; private set; }
        public CircuitBreaker CircuitBreaker { get; private set; }

        public CommandGroup(string name)
        {
            Name = name;
            CircuitBreaker = new CircuitBreaker();
        }
    }
}
