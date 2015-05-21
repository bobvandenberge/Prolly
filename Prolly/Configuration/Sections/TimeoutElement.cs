using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Prolly.Configuration.Sections
{
    /// <summary>
    /// Used for the configuration of the Timeout
    /// </summary>
    public class TimeoutElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets amount of miliseconds the timeout will wait before throwing an exception
        /// </summary>
        /// <value>
        /// The miliseconds the timeout will wait before throwing an exception
        /// </value>
        [ConfigurationProperty("miliseconds", DefaultValue = "1000", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MinValue = 100)]
        public int Miliseconds
        {
            get
            { return (int) this["miliseconds"]; }
        }
    }
}
