using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Prolly.Configuration.Sections
{
    /// <summary>
    /// Used for the configuration of the CircuitBreaker
    /// </summary>
    public class CircuitBreakerElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the amount of times a failure is allowed before the circuit breaker opens
        /// </summary>
        /// <value>
        /// The amount of times a failure is allowed before the circuit breaker opens
        /// </value>
        [ConfigurationProperty("allowedFailures", DefaultValue = "2", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MinValue = 0)]
        public int AllowedFailures
        {
            get
            { return (int) this["allowedFailures"]; }
            set
            { this["allowedFailures"] = value; }
        }

        /// <summary>
        /// Gets or sets the time in miliseconds the circuit breaker will remain open. Once the time passes it wil go into the HalfOpen state
        /// </summary>
        /// <value>
        /// The time in miliseconds the circuit breaker will remain open. Once the time passes it wil go into the HalfOpen state.
        /// </value>
        [ConfigurationProperty("openTimeInMiliseconds", DefaultValue = "1000", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MinValue = 100)]
        public int OpenTimeInMiliseconds
        {
            get
            { return (int) this["openTimeInMiliseconds"]; }
            set
            { this["openTimeInMiliseconds"] = value; }
        }
    }
}
