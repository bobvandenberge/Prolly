using Prolly.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Patterns.CircuitBreaker
{
    public class SimpleCircuitBreaker : ICircuitBreaker
    {
        private CircuitBreakerState _state = CircuitBreakerState.Closed;
        private int _allowedFailures;
        private TimeSpan _timeOpen;
        private int _currentFailureCount;

        public SimpleCircuitBreaker(int allowedFailures, TimeSpan openTime)
        {
            _allowedFailures = allowedFailures;
            _timeOpen = openTime;
            _currentFailureCount = 0;
        }

        public SimpleCircuitBreaker()
            :this (CircuitBreakerConfiguration.AllowedFailures, CircuitBreakerConfiguration.TimeOpen)
        { }

        public bool AllowRequest
        {
            get
            {
                return _state == CircuitBreakerState.Closed || _state == CircuitBreakerState.HalfOpen;
            }
        }

        public void MarkSucces()
        {
            if ( _state == CircuitBreakerState.HalfOpen )
            {
                _state = CircuitBreakerState.Closed;
                _currentFailureCount = 0;
            }
        }

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
