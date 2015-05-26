using Prolly.Commands;
using Prolly.Configuration;
using Prolly.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Patterns
{
    /// <summary>
    /// Used the implemented the Bulkhead pattern
    /// </summary>
    public abstract class AbstractBulkhead
    {
        /// <summary>
        /// Whether or not a new Task is allowed to run
        /// </summary>
        public abstract bool HasRoom { get; }

        /// <summary>
        /// Register that a task has been started
        /// </summary>
        public abstract void TaskStarted();

        /// <summary>
        /// Register that a task has finished
        /// </summary>
        public abstract void TaskFinished();

        public static class Factory
        {
            private static ConcurrentDictionary<CommandGroupKey, AbstractBulkhead> _bulkheads = new ConcurrentDictionary<CommandGroupKey, AbstractBulkhead>();

            /// <summary>
            /// Resolves an AbstractBulkhead instance
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The bulkhead</returns>
            public static AbstractBulkhead Resolve(CommandGroupKey key)
            {
                if ( !_bulkheads.ContainsKey(key) )
                    _bulkheads[key] = new SimpleBulkhead();

                return _bulkheads[key];
            }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public static void Reset()
            {
                _bulkheads.Clear();
            }

            /// <summary>
            /// Simple bulkhead implementation
            /// </summary>
            private class SimpleBulkhead : AbstractBulkhead
            {
                private object _lock = new object();

                /// <summary>
                /// The amount of tasks that are allowed to run concurrent
                /// </summary>
                public int ConcurrentTasks { get; private set; }

                /// <summary>
                /// Whether or not a new Task is allowed to run
                /// </summary>
                public override bool HasRoom
                {
                    get
                    {
                        if ( ConcurrentTasks == 0 )
                            return true;

                        return CurrentlyActiveTasks < ConcurrentTasks;
                    }
                }

                /// <summary>
                /// The amount of tasks that are currently active
                /// </summary>
                public int CurrentlyActiveTasks { get; private set; }

                /// <summary>
                /// Initializes a new instance of the <see cref="SimpleBulkhead{T}"/> class.
                /// </summary>
                public SimpleBulkhead()
                    : this(BulkheadConfiguration.ConcurrentTasks)
                { }

                /// <summary>
                /// Initializes a new instance of the <see cref="SimpleBulkhead{T}"/> class.
                /// </summary>
                /// <param name="concurrentTasks">The amount of tasks that are allowed to run concurrent</param>
                public SimpleBulkhead(int concurrentTasks)
                {
                    ConcurrentTasks = concurrentTasks;

                    if ( concurrentTasks < 0 )
                    {
                        throw new ArgumentOutOfRangeException("concurrentTasks", "Must be zero or a positive number.");
                    }
                }

                /// <summary>
                /// Register that a task has been started
                /// </summary>
                public override void TaskStarted()
                {
                    lock ( _lock )
                    {
                        if ( !HasRoom )
                            throw new MaximumAllowedTasksReachedException("A task cannot be started. Maximum amount of tasks already reached.");

                        CurrentlyActiveTasks++;
                    }
                }

                /// <summary>
                /// Register that a task has finished
                /// </summary>
                public override void TaskFinished()
                {
                    lock ( _lock )
                    {
                        CurrentlyActiveTasks--;
                    }
                }
            }
        }
    }
}
