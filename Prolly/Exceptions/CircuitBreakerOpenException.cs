using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Exceptions
{
    /// <summary>
    /// Exception that gets thrown when trying to send a request through 
    /// a CircuitBreaker while it's open.
    /// </summary>
    [Serializable]
    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException() { }
        public CircuitBreakerOpenException(string message) : base(message) { }
        public CircuitBreakerOpenException(string message, Exception inner) : base(message, inner) { }
        protected CircuitBreakerOpenException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
