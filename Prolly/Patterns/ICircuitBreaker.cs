using System;

namespace Prolly.Patterns
{
    /// <summary>
    /// Used to implement the CircuirBreaker
    /// </summary>
    public interface ICircuitBreaker
    {
        /// <summary>
        /// Gets a value indicating whether to allow requests.
        /// </summary>
        /// <value>
        ///   <c>true</c> if requests are allowed; otherwise, <c>false</c>.
        /// </value>
        bool AllowRequest { get; }

        /// <summary>
        /// A message was handeld succesfully. If the CircuitBreaker is in an
        /// HalfOpen state then the CircuitBreaker will be restored
        /// </summary>
        void MarkSucces();

        /// <summary>
        /// Try to break the CircuitBreaker. This will succeed if the number of
        /// failures surpasses the Threshold
        /// </summary>
        void TryBreak();
    }
}
