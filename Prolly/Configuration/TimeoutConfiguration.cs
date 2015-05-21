using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Configuration
{
    /// <summary>
    /// Default configuration that ITimeout implementations should use
    /// </summary>
    public static class TimeoutConfiguration
    {
        /// <summary>
        /// Gets the waiting time.
        /// </summary>
        /// <value>
        /// The waiting time.
        /// </value>
        public static TimeSpan WaitingTime
        {
            get
            {
                return TimeSpan.FromMilliseconds(ProllyConfiguration.ProllySection.Timeout.Miliseconds);
            }
        }
    }
}
