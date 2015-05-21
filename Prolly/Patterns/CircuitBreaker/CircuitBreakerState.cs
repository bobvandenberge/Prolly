using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Patterns.CircuitBreaker
{
    public enum CircuitBreakerState
    {
        Open,
        Closed,
        HalfOpen
    }
}
