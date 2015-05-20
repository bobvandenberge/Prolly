using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Exceptions
{
    [Serializable]
    public class TaskNotStartedException : Exception
    {
        public TaskNotStartedException() { }
        public TaskNotStartedException(string message) : base(message) { }
        public TaskNotStartedException(string message, Exception inner) : base(message, inner) { }
        protected TaskNotStartedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
