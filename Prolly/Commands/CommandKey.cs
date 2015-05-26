using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Commands
{
    /// <summary>
    /// A key to represent a ProllyCommand for monitoring, circuit-breakers etc
    /// </summary>
    public abstract class CommandKey
    {
        /// <summary>
        /// The name of the command
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Factory for creating and resolving CommandKeys
        /// </summary>
        public static class Factory
        {
            private static ConcurrentDictionary<string, CommandKey> _commandKeys = new ConcurrentDictionary<string, CommandKey>();

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public static void Reset()
            {
                _commandKeys.Clear();
            }

            /// <summary>
            /// Resolves a CommandKey by its name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>The CommandKey</returns>
            public static CommandKey Resolve(string name)
            {
                if ( !_commandKeys.ContainsKey(name) )
                {
                    _commandKeys[name] = new DefaultCommandKey(name);
                }

                return _commandKeys[name];
            }

            /// <summary>
            /// Default implementation of the CommandKey
            /// </summary>
            private class DefaultCommandKey : CommandKey
            {
                private string _name;

                /// <summary>
                /// The name of the command
                /// </summary>
                public override string Name { get { return _name; } }

                /// <summary>
                /// Initializes a new instance of the <see cref="DefaultCommandKey"/> class.
                /// </summary>
                /// <param name="name">The name.</param>
                public DefaultCommandKey(string name)
                {
                    _name = name;
                }
            }
        }
    }
}
