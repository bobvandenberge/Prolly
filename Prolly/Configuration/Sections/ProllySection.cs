using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Configuration.Sections
{
    /// <summary>
    /// Used for the configuration of Prolly
    /// </summary>
    public class ProllySection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        [ConfigurationProperty("timeout")]
        public TimeoutElement Timeout
        {
            get
            {
                return (TimeoutElement) this["timeout"];
            }
            set
            { this["timeout"] = value; }
        }

        /// <summary>
        /// Gets or sets the circuit breaker.
        /// </summary>
        /// <value>
        /// The circuit breaker.
        /// </value>
        [ConfigurationProperty("circuitBreaker")]
        public CircuitBreakerElement CircuitBreaker
        {
            get
            {
                return (CircuitBreakerElement) this["circuitBreaker"];
            }
            set
            { this["circuitBreaker"] = value; }
        }
    }

    
}
