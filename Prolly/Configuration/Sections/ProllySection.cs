using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Configuration.Sections
{
    public class ProllySection : ConfigurationSection
    {
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

    public class TimeoutElement : ConfigurationElement
    {
        [ConfigurationProperty("miliseconds", DefaultValue = "1000", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MinValue = 100)]
        public int Miliseconds
        {
            get
            { return (int) this["miliseconds"]; }
            set
            { this["miliseconds"] = value; }
        }
    }

    public class CircuitBreakerElement : ConfigurationElement
    {
        [ConfigurationProperty("allowedFailures", DefaultValue = "2", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MinValue = 0)]
        public int AllowedFailures
        {
            get
            { return (int) this["allowedFailures"]; }
            set
            { this["allowedFailures"] = value; }
        }

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
