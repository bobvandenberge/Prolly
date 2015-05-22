using Prolly.Configuration;
using Prolly.Patterns.CircuitBreaker.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Patterns.CircuitBreaker
{
    /// <summary>
    /// A simple implementation of the ICircuitBreaker
    /// </summary>
    public class SimpleCircuitBreaker : ICircuitBreaker
    {
        public ICircuitBreakerState State { get; set; }
        public HealthPolicy Policy { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCircuitBreaker"/> class.
        /// </summary>
        /// <param name="allowedFailures">The amount of times a failure is allowed before the circuit breaker opens</param>
        /// <param name="timeOpen">The time in miliseconds the circuit breaker will remain open. Once the time passes it wil go into the HalfOpen state.</param>
        public SimpleCircuitBreaker(int allowedFailures, TimeSpan timeOpen)
        {
            State = new ClosedState(this);
            Policy = new HealthPolicy(allowedFailures, timeOpen);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCircuitBreaker"/> class.
        /// </summary>
        public SimpleCircuitBreaker()
            :this (CircuitBreakerConfiguration.AllowedFailures, CircuitBreakerConfiguration.TimeOpen)
        { }

        /// <summary>
        /// Gets a value indicating whether to allow requests.
        /// </summary>
        /// <value>
        ///   <c>true</c> if requests are allowed; otherwise, <c>false</c>.
        /// </value>
        public bool AllowRequests
        {
            get
            {
                return State.AllowRequests;
            }
        }

        /// <summary>
        /// A message was handeld succesfully. If the CircuitBreaker is in an
        /// HalfOpen state then the CircuitBreaker will be restored
        /// </summary>
        public void TryRestore()
        {
            if (State is HalfOpenState)
            {
                State = new ClosedState(this);
                Policy.Reset();
            }
        }

        /// <summary>
        /// Try to break the CircuitBreaker. This will succeed if the number of
        /// failures surpasses the Threshold
        /// </summary>
        public void TryBreak()
        {
            Policy.FailureOccured();

            if(!Policy.IsHealthy)
            {
                State = new OpenState(this);
            }
        }
    }
}
