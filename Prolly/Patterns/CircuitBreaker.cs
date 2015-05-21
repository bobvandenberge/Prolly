using Prolly.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Patterns
{
    public class CircuitBreaker
    {
        private CircuitBreakerState _state = CircuitBreakerState.Closed;

        public int CurrentFailureCount { get; private set; }
        public int AllowedFailures { get; private set; }
        public TimeSpan TimeOpen { get; private set; }

        public CircuitBreaker(int allowedFailures, TimeSpan openTime)
        {
            AllowedFailures = allowedFailures;
            TimeOpen = openTime;
            CurrentFailureCount = 0;

        }

        public CircuitBreaker()
            :this (CircuitBreakerConfiguration.AllowedFailures, CircuitBreakerConfiguration.TimeOpen)
        { }

        public bool IsOpen 
        { 
            get 
            { 
                return _state == CircuitBreakerState.Open; 
            } 
        }

        public bool IsClosed
        {
            get
            {
                return _state == CircuitBreakerState.Closed;
            }
        }

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
                CurrentFailureCount = 0;
            }
        }

        public void TryBreak()
        {
            if(++CurrentFailureCount >= AllowedFailures)
            {
                _state = CircuitBreakerState.Open;
                StartOpenTimer();
            }
        }

        private void StartOpenTimer()
        {
            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(TimeOpen);

                if(_state == CircuitBreakerState.Open)
                    _state = CircuitBreakerState.HalfOpen;
            });
        }

        public void ForceOpen()
        {
            _state = CircuitBreakerState.Open;
        }

        public void ForceClose()
        {
            _state = CircuitBreakerState.Closed;
        }
    }

    public enum CircuitBreakerState
    {
        Open,
        Closed,
        HalfOpen
    }
}
