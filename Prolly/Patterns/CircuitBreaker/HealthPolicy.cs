using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Patterns.CircuitBreaker
{
    /// <summary>
    /// A health polciy that can tell you if the thing it is monitoring
    /// is currently healthy
    /// </summary>
    public class HealthPolicy
    {
        private int _currentFailureCount = 0;
        private int _allowedFailures;
        private TimeSpan _timeOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthPolicy"/> class.
        /// </summary>
        /// <param name="allowedFailures">The amount of times a failure is allowed before the circuit breaker opens</param>
        /// <param name="timeOpen"></param>
        public HealthPolicy(int allowedFailures, TimeSpan timeOpen)
        {
            _allowedFailures = allowedFailures;
            _timeOpen = timeOpen;
        }

        /// <summary>
        /// A value indicating whether or not the current state is healthy
        /// </summary>
        public bool IsHealthy
        {
            get
            {
                return !(_currentFailureCount >= _allowedFailures);
            }
        }

        /// <summary>
        /// The time the circuit is supposed to stay open before returning to the half open state
        /// </summary>
        public TimeSpan TimeOpen
        {
            get
            {
                return _timeOpen;
            }
        }

        /// <summary>
        /// Indicate that a failure has occured
        /// </summary>
        public void FailureOccured()
        {
            _currentFailureCount++;
        }

        /// <summary>
        /// Reset the metrics
        /// </summary>
        public void Reset()
        {
            _currentFailureCount = 0;
        }
    }
}
