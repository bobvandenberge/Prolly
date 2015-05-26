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
        /// Gets the amount of succesful tries.
        /// </summary>
        public abstract int Succes { get; }

        /// <summary>
        /// Gets the amount of failed tries
        /// </summary>
        public abstract int Failure { get; }

        /// <summary>
        /// Gets the error percentage.
        /// </summary>
        public abstract int ErrorPercentage { get; }

        /// <summary>
        /// Indicate that a failure has occured
        /// </summary>
        public abstract void MarkFailure();

        /// <summary>
        /// Indicate that a action was succesfull
        /// </summary>
        public abstract void MarkSucces();

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
                private int _succes;
                private int _failure;

                /// <summary>
                /// Gets the amount of succesful tries.
                /// </summary>
                public override int Succes
                {
                    get { return _succes; }
                }

                /// <summary>
                /// Gets the amount of failed tries
                /// </summary>
                public override int Failure
                {
                    get { return _failure; }
                }

                /// <summary>
                /// Gets the error percentage.
                /// </summary>
                public override int ErrorPercentage
                {
                    get 
                    {
                        if ( _failure == 0 && _succes != 0 )
                            return 0;

                        if ( _failure != 0 && _succes == 0 )
                            return 100;

                        return (_failure / (_succes + _failure)) * 100 ;
                    }
                }

                /// <summary>
                /// Indicate that a failure has occured
                /// </summary>
                public override void MarkFailure()
                {
                    _failure++;
                }

                /// <summary>
                /// Indicate that a action was succesfull
                /// </summary>
                public override void MarkSucces()
                {
                    _succes++;
                }

                /// <summary>
                /// Reset the metrics
                /// </summary>
                public override void Reset()
                {
                    _succes = 0;
                    _failure = 0;
                }
            }
        }
    }
}
