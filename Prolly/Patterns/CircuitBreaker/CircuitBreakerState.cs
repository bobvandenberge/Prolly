using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Patterns.CircuitBreaker
{
    /// <summary>
    /// The States the CircuitBreaker can have
    /// </summary>
    public enum CircuitBreakerState
    {
        /// <summary>
        /// CircuitBreaker is not allowing messages
        /// </summary>
        Open,

        /// <summary>
        /// CircuitBreaker is allowing messages
        /// </summary>
        Closed,

        /// <summary>
        /// CircuitBreaker allows one message
        /// </summary>
        HalfOpen
    }
}
