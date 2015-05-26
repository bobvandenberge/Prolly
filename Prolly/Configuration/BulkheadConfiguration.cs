using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly.Configuration
{
    /// <summary>
    /// Default configuration for the Bulkhead implementation
    /// </summary>
    public static class BulkheadConfiguration
    {
        /// <summary>
        /// Gets the allowed failures.
        /// </summary>
        /// <value>
        /// The allowed failures.
        /// </value>
        public static int ConcurrentTasks
        {
            get
            {
                return ProllyConfiguration.ProllySection.Bulkhead.ConcurrentTasks;
            }
        }
    }
}
