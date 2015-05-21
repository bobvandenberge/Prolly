using Prolly.Patterns.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Commands
{
    /// <summary>
    /// Used to manage CommandGroups.
    /// </summary>
    public static class CommandGroupFactory
    {
        private static Dictionary<string, CommandGroup> groups = new Dictionary<string, CommandGroup>();

        /// <summary>
        /// Resolves a command group by its name.
        /// </summary>
        /// <param name="commandGroupName">Name of the command group.</param>
        /// <returns>A command group instance</returns>
        public static CommandGroup Resolve(string commandGroupName)
        {
            if ( commandGroupName == String.Empty )
                return new CommandGroup(String.Empty, new SimpleCircuitBreaker());

            if ( !groups.ContainsKey(commandGroupName) )
                groups.Add(commandGroupName, new CommandGroup(commandGroupName, new SimpleCircuitBreaker()));
                
            return groups[commandGroupName];
        }

        /// <summary>
        /// Remove all CommandGroups that are currently known to the factory
        /// </summary>
        public static void Reset()
        {
            groups.Clear();
        }
    }
}
