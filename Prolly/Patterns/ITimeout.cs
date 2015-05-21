using System;

namespace Prolly.Patterns
{
    /// <summary>
    /// Used to implement the Timeout Pattern
    /// </summary>
    public interface ITimeout
    {
        /// <summary>
        /// Monitors the specified task to see if it times out
        /// </summary>
        /// <param name="task">The task to monitor</param>
        void Monitor(System.Threading.Tasks.Task task);       
    }
}
