using Prolly.Commands;
using Prolly.Configuration;
using Prolly.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Prolly.Patterns
{
    /// <summary>
    /// Used to implement the Timeout Pattern
    /// </summary>
    public abstract class AbstractTimeout
    {
        /// <summary>
        /// Monitors the specified task to see if it times out
        /// </summary>
        /// <param name="task">The task to monitor</param>
        public abstract void Monitor(Task task);       

        public static class Factory
        {
            private static ConcurrentDictionary<CommandGroupKey, AbstractTimeout> _timeouts = new ConcurrentDictionary<CommandGroupKey, AbstractTimeout>();

            /// <summary>
            /// Resolves an AbstractTimeout instance
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The timeout</returns>
            public static AbstractTimeout Resolve(CommandGroupKey key)
            {
                if ( !_timeouts.ContainsKey(key) )
                    _timeouts[key] = new SimpleTimeout();

                return _timeouts[key];
            }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public static void Reset()
            {
                _timeouts.Clear();
            }

            /// <summary>
            /// A simple implementation of the ITimeout
            /// </summary>
            private class SimpleTimeout : AbstractTimeout
            {
                private TimeSpan _waitingTime;

                /// <summary>
                /// Initializes a new instance of the <see cref="SimpleTimeout"/> class.
                /// </summary>
                /// <param name="waitingTime">The miliseconds the timeout will wait before throwing an exception</param>
                public SimpleTimeout()
                {
                    _waitingTime = TimeoutConfiguration.WaitingTime;
                }

                /// <summary>
                /// Monitors the specified task to see if it times out
                /// </summary>
                /// <param name="task">The task to monitor</param>
                /// <exception cref="Prolly.Exceptions.TaskNotStartedException">The task has to be started in order to monitor it.</exception>
                /// <exception cref="TimeoutException">The task didn't complete in time.</exception>
                public override void Monitor(Task task)
                {
                    if ( task.Status == TaskStatus.Created )
                        throw new TaskNotStartedException("The task has to be started in order to monitor it.");

                    if ( !task.Wait((int) _waitingTime.TotalMilliseconds) )
                    {
                        throw new TimeoutException("The task didn't complete in time.");
                    }
                }
            }
        }
    }
}
