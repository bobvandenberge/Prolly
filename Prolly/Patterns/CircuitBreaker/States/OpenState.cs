using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Patterns.CircuitBreaker.States
{
    /// <summary>
    /// The Open State for a Circuit Breaker. No requests are allowed now
    /// </summary>
    internal class OpenState : ICircuitBreakerState
    {
        private SimpleCircuitBreaker _circuitBreaker;
        private DateTime exitDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenState"/> class.
        /// </summary>
        /// <param name="circuitBreaker">The circuit breaker this state belongs to</param>
        public OpenState(SimpleCircuitBreaker circuitBreaker)
        {
            _circuitBreaker = circuitBreaker;
            exitDate = DateTime.Now + circuitBreaker.Policy.TimeOpen;
        }

        /// <summary>
        /// Gets a value indicating whether to allow requests.
        /// </summary>
        public bool AllowRequests
        {
            get 
            {
                if (DateTime.Now > exitDate)
                {
                    _circuitBreaker.State = new HalfOpenState(_circuitBreaker);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
