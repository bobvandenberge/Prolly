using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Exceptions
{
    /// <summary>
    /// Exception that gets thrown when the maximum allowed of tasks
    /// is reached within an task schedular
    /// </summary>
    [Serializable]
    public class MaximumAllowedTasksReachedException : Exception
    {
        public MaximumAllowedTasksReachedException() { }
        public MaximumAllowedTasksReachedException(string message) : base(message) { }
        public MaximumAllowedTasksReachedException(string message, Exception inner) : base(message, inner) { }
        protected MaximumAllowedTasksReachedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
