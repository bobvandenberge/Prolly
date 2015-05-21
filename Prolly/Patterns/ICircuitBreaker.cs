using System;

namespace Prolly.Patterns
{
    public interface ICircuitBreaker
    {
        bool AllowRequest { get; }
        void MarkSucces();
        void TryBreak();
    }
}
