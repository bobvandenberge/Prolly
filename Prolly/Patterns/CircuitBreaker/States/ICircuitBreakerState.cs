using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Patterns.CircuitBreaker.States
{
    /// <summary>
    /// The interface for ciruit breaker states
    /// </summary>
    public interface ICircuitBreakerState
    {
        /// <summary>
        /// Gets a value indicating whether to allow requests.
        /// </summary>
        bool AllowRequests { get; }
    }
}
