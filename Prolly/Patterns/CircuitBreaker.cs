using Prolly.Commands;
using Prolly.Configuration;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Prolly.Patterns
{
    /// <summary>
    /// Used to implement the CircuirBreaker
    /// </summary>
    public abstract class AbstractCircuitBreaker
    {
        /// <summary>
        /// Gets a value indicating whether to allow requests.
        /// </summary>
        /// <value>
        ///   <c>true</c> if requests are allowed; otherwise, <c>false</c>.
        /// </value>
        public abstract bool AllowRequests { get; }

        /// <summary>
        /// A message was handeld succesfully. If the CircuitBreaker is in an
        /// HalfOpen state then the CircuitBreaker will be restored
        /// </summary>
        public abstract void MarkSucces();

        /// <summary>
        /// Try to break the CircuitBreaker. This will succeed if the number of
        /// failures surpasses the Threshold
        /// </summary>
        public abstract void MarkFailure();

        public static class Factory
        {
            private static ConcurrentDictionary<CommandGroupKey, AbstractCircuitBreaker> _circuitBreakers = new ConcurrentDictionary<CommandGroupKey, AbstractCircuitBreaker>();

            /// <summary>
            /// Resolves an AbstractCircuitBreaker instance
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The circuit breaker</returns>
            public static AbstractCircuitBreaker Resolve(CommandGroupKey key)
            {
                if ( !_circuitBreakers.ContainsKey(key) )
                    _circuitBreakers[key] = new SimpleCircuitBreaker(key);

                return _circuitBreakers[key];
            }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public static void Reset()
            {
                _circuitBreakers.Clear();
            }

            /// <summary>
            /// A simple implementation of the ICircuitBreaker
            /// </summary>
            private class SimpleCircuitBreaker : AbstractCircuitBreaker
            {
                public string State { get; set; }
                public CommandMetrics Metrics { get; private set; }

                /// <summary>
                /// Initializes a new instance of the <see cref="SimpleCircuitBreaker" /> class.
                /// </summary>
                /// <param name="key">The key.</param>
                public SimpleCircuitBreaker(CommandGroupKey key)
                {
                    State = "closed";
                    Metrics = CommandMetrics.Factory.Resolve(key);
                }

                /// <summary>
                /// Gets a value indicating whether to allow requests.
                /// </summary>
                /// <value>
                ///   <c>true</c> if requests are allowed; otherwise, <c>false</c>.
                /// </value>
                public override bool AllowRequests
                {
                    get
                    {
                        return State == "closed" || State == "half";
                    }
                }

                /// <summary>
                /// A message was handeld succesfully. If the CircuitBreaker is in an
                /// HalfOpen state then the CircuitBreaker will be restored
                /// </summary>
                public override void MarkSucces()
                {
                    Metrics.MarkSucces();
                    if ( State == "half" )
                    {
                        State = "closed";
                        Metrics.Reset();
                    }
                }

                /// <summary>
                /// Try to break the CircuitBreaker. This will succeed if the number of
                /// failures surpasses the Threshold
                /// </summary>
                public override void MarkFailure()
                {
                    Metrics.MarkFailure();

                    if ( Metrics.Failure >= 2 )
                    {
                        State = "open";
                        StartTimer();
                    }
                }

                private void StartTimer()
                {
                    Task.Factory.StartNew(() =>
                    {
                        System.Threading.Thread.Sleep(CircuitBreakerConfiguration.TimeOpen);
                        State = "half";
                    });
                }
            }
        }
    }
}
