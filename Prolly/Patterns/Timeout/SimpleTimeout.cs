using Prolly.Configuration;
using Prolly.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prolly.Patterns.Timeout
{
    /// <summary>
    /// A simple implementation of the ITimeout
    /// </summary>
    public class SimpleTimeout : ITimeout
    {
        private TimeSpan _waitingTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTimeout"/> class.
        /// </summary>
        /// <param name="waitingTime">The miliseconds the timeout will wait before throwing an exception</param>
        public SimpleTimeout(TimeSpan waitingTime)
        {
            _waitingTime = waitingTime;
        }

        /// <summary>
        /// Monitors the specified task to see if it times out
        /// </summary>
        /// <param name="task">The task to monitor</param>
        /// <exception cref="Prolly.Exceptions.TaskNotStartedException">The task has to be started in order to monitor it.</exception>
        /// <exception cref="TimeoutException">The task didn't complete in time.</exception>
        public void Monitor(Task task)
        {
            if ( task.Status == TaskStatus.Created )
                throw new TaskNotStartedException("The task has to be started in order to monitor it.");

            var cancellationTokenSource = new CancellationTokenSource();
            if ( !task.Wait((int) _waitingTime.TotalMilliseconds, cancellationTokenSource.Token) )
            {
                cancellationTokenSource.Cancel();
                throw new TimeoutException("The task didn't complete in time.");
            }
        }
    }
}
