using Prolly.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Commands
{
    /// <summary>
    /// Class for collecting the metrics of a command
    /// </summary>
    public abstract class CommandMetrics
    {
        /// <summary>
        /// A value indicating whether or not the current state is healthy
        /// </summary>
        public abstract bool IsHealthy { get; }

        /// <summary>
        /// The time the circuit is supposed to stay open before returning to the half open state
        /// </summary>
        public abstract TimeSpan TimeOpen { get; }

        /// <summary>
        /// Indicate that a failure has occured
        /// </summary>
        public abstract void FailureOccured();

        /// <summary>
        /// Reset the metrics
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Factory to resolve CommandMetrics instances
        /// </summary>
        public static class Factory
        {
            private static ConcurrentDictionary<CommandGroupKey, CommandMetrics> _metrics = new ConcurrentDictionary<CommandGroupKey, CommandMetrics>();

            /// <summary>
            /// Resolves a CommandMetrics instance
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The metrics</returns>
            public static CommandMetrics Resolve(CommandGroupKey key)
            {
                if ( !_metrics.ContainsKey(key) )
                    _metrics[key] = new SimpleCommandMetrics();

                return _metrics[key];
            }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public static void Reset()
            {
                _metrics.Clear();
            }

            private class SimpleCommandMetrics : CommandMetrics
            {
                private int _currentFailureCount = 0;
                private int _allowedFailures;
                private TimeSpan _timeOpen;

                /// <summary>
                /// Initializes a new instance of the <see cref="SimpleCommandMetrics"/> class.
                /// </summary>
                public SimpleCommandMetrics()
                {
                    _allowedFailures = CircuitBreakerConfiguration.AllowedFailures;
                    _timeOpen = CircuitBreakerConfiguration.TimeOpen;
                }

                /// <summary>
                /// A value indicating whether or not the current state is healthy
                /// </summary>
                public override bool IsHealthy
                {
                    get
                    {
                        return !(_currentFailureCount >= _allowedFailures);
                    }
                }

                /// <summary>
                /// The time the circuit is supposed to stay open before returning to the half open state
                /// </summary>
                public override TimeSpan TimeOpen
                {
                    get
                    {
                        return _timeOpen;
                    }
                }

                /// <summary>
                /// Indicate that a failure has occured
                /// </summary>
                public override void FailureOccured()
                {
                    _currentFailureCount++;
                }

                /// <summary>
                /// Reset the metrics
                /// </summary>
                public override void Reset()
                {
                    _currentFailureCount = 0;
                }
            }
        }
    }
}
