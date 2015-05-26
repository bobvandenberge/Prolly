using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Commands
{
    /// <summary>
    /// A group name for a ProllyCommand. This is used for grouping together commands.
    /// </summary>
    public abstract class CommandGroupKey
    {
        /// <summary>
        /// The name of the group
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Factory for creating and resolving CommandGroupKeys
        /// </summary>
        public static class Factory
        {
            private static ConcurrentDictionary<string, CommandGroupKey> _commandGroupKeys = new ConcurrentDictionary<string, CommandGroupKey>();

            /// <summary>
            /// Resolves a CommandGroupKey by its name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>The CommandGroupKey</returns>
            public static CommandGroupKey Resolve(string name)
            {
                if ( !_commandGroupKeys.ContainsKey(name) )
                {
                    _commandGroupKeys[name] = new DefaultCommandGroupKey(name);
                }

                return _commandGroupKeys[name];
            }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public static void Reset()
            {
                _commandGroupKeys.Clear();
            }

            /// <summary>
            /// Default implementation of the CommandGroupKey
            /// </summary>
            private class DefaultCommandGroupKey : CommandGroupKey
            {
                private string _name;

                /// <summary>
                /// The name of the command
                /// </summary>
                public override string Name { get { return _name; } }

                /// <summary>
                /// Initializes a new instance of the <see cref="DefaultCommandGroupKey"/> class.
                /// </summary>
                /// <param name="name">The name.</param>
                public DefaultCommandGroupKey(string name)
                {
                    _name = name;
                }
            }
        }
    }
}
