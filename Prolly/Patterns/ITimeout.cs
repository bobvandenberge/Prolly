using System;

namespace Prolly.Patterns
{
    public interface ITimeout
    {
        TimeSpan WaitingTime { get; }

        void Monitor(System.Threading.Tasks.Task task);       
    }
}
