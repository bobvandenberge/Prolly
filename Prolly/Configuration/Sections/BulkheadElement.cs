using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Prolly.Configuration.Sections
{
    /// <summary>
    /// Used for the configuration of the Bulkhead
    /// </summary>
    public class BulkheadElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the amount of tasks that a command group is allowed to run concurrently.
        /// 0 == Infinite
        /// </summary>
        /// <value>
        /// The amount of tasks that a command group is allowed to run concurrently
        /// </value>
        [ConfigurationProperty("concurrentTasks", DefaultValue = "10", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MinValue = 0)]
        public int ConcurrentTasks
        {
            get
            { return (int)this["concurrentTasks"]; }
        }
    }
}
