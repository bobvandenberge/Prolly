using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Configuration
{
    /// <summary>
    /// Default configuration that ICircuitBreaker implementations should use
    /// </summary>
    public static class CircuitBreakerConfiguration
    {
        /// <summary>
        /// Gets the time open.
        /// </summary>
        /// <value>
        /// The time open.
        /// </value>
        public static TimeSpan TimeOpen
        {
            get
            {
                return TimeSpan.FromMilliseconds(ProllyConfiguration.ProllySection.CircuitBreaker.OpenTimeInMiliseconds);
            }
        }

        /// <summary>
        /// Gets the allowed failures.
        /// </summary>
        /// <value>
        /// The allowed failures.
        /// </value>
        public static int AllowedFailures
        {
            get
            {
                return ProllyConfiguration.ProllySection.CircuitBreaker.AllowedFailures;
            }
        }
    }
}
