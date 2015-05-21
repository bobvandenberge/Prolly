using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Exceptions
{
    [Serializable]
    public class CircuitBreakerIgnoreException : Exception
    {
        public CircuitBreakerIgnoreException() { }
        public CircuitBreakerIgnoreException(string message) : base(message) { }
        public CircuitBreakerIgnoreException(string message, Exception inner) : base(message, inner) { }
        protected CircuitBreakerIgnoreException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
