using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Patterns.CircuitBreaker.States
{
    /// <summary>
    /// The Closed State for a Circuit Breaker. Requests are allowed now
    /// </summary>
    internal class ClosedState : ICircuitBreakerState
    {
        private SimpleCircuitBreaker _circuitBreaker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClosedState"/> class.
        /// </summary>
        /// <param name="circuitBreaker">The circuit breaker this state belongs to</param>
        public ClosedState(SimpleCircuitBreaker circuitBreaker)
        {
            _circuitBreaker = circuitBreaker;
        }

        /// <summary>
        /// Gets a value indicating whether to allow requests.
        /// </summary>
        public bool AllowRequests
        {
            get { return true; }
        }
    }
}
