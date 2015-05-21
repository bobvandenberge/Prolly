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
    public class SimpleTimeout : ITimeout
    {
        public TimeSpan WaitingTime { get; private set; }
        
        public SimpleTimeout(TimeSpan waitingTime)
        {
            WaitingTime = waitingTime;
        }

        public void Monitor(Task task)
        {
            if ( task.Status == TaskStatus.Created )
                throw new TaskNotStartedException("The task has to be started in order to monitor it.");

            var cancellationTokenSource = new CancellationTokenSource();
            if ( !task.Wait((int) WaitingTime.TotalMilliseconds, cancellationTokenSource.Token) )
            {
                cancellationTokenSource.Cancel();
                throw new TimeoutException();
            }
        }
    }
}
