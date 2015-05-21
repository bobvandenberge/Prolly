using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Exceptions
{
    /// <summary>
    /// Exception that gets thrown when an ProllyCommand hasn't
    /// specified a fallback
    /// </summary>
    [Serializable]
    public class FallbackNotSpecifiedException : Exception
    {
        public FallbackNotSpecifiedException() { }
        public FallbackNotSpecifiedException(string message) : base(message) { }
        public FallbackNotSpecifiedException(string message, Exception inner) : base(message, inner) { }
        protected FallbackNotSpecifiedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
