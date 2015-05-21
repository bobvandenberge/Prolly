using Prolly.Configuration;
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
        private CircuitBreakerState _state = CircuitBreakerState.Closed;
        private int _allowedFailures;
        private TimeSpan _timeOpen;
        private int _currentFailureCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCircuitBreaker"/> class.
        /// </summary>
        /// <param name="allowedFailures">The amount of times a failure is allowed before the circuit breaker opens</param>
        /// <param name="openTime">The time in miliseconds the circuit breaker will remain open. Once the time passes it wil go into the HalfOpen state.</param>
        public SimpleCircuitBreaker(int allowedFailures, TimeSpan openTime)
        {
            _allowedFailures = allowedFailures;
            _timeOpen = openTime;
            _currentFailureCount = 0;
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
        public bool AllowRequest
        {
            get
            {
                return _state == CircuitBreakerState.Closed || _state == CircuitBreakerState.HalfOpen;
            }
        }

        /// <summary>
        /// A message was handeld succesfully. If the CircuitBreaker is in an
        /// HalfOpen state then the CircuitBreaker will be restored
        /// </summary>
        public void MarkSucces()
        {
            if ( _state == CircuitBreakerState.HalfOpen )
            {
                _state = CircuitBreakerState.Closed;
                _currentFailureCount = 0;
            }
        }

        /// <summary>
        /// Try to break the CircuitBreaker. This will succeed if the number of
        /// failures surpasses the Threshold
        /// </summary>
        public void TryBreak()
        {
            if(++_currentFailureCount >= _allowedFailures)
            {
                _state = CircuitBreakerState.Open;
                StartOpenTimer();
            }
        }

        private void StartOpenTimer()
        {
            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(_timeOpen);

                if(_state == CircuitBreakerState.Open)
                    _state = CircuitBreakerState.HalfOpen;
            });
        }
    }
}
