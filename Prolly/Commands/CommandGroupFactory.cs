using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Commands
{
    public class CommandGroupFactory
    {
        private static Dictionary<string, CommandGroup> groups = new Dictionary<string, CommandGroup>();

        public static CommandGroup Resolve(string commandGroupName)
        {
            if ( commandGroupName == String.Empty )
                return new CommandGroup(String.Empty);

            if ( !groups.ContainsKey(commandGroupName) )
                groups.Add(commandGroupName, new CommandGroup(commandGroupName));
                
            return groups[commandGroupName];
        }
    }
}
