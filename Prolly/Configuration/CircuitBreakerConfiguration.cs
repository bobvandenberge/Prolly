using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Configuration
{
    public static class CircuitBreakerConfiguration
    {
        public static TimeSpan TimeOpen
        {
            get
            {
                return TimeSpan.FromMilliseconds(ProllyConfiguration.ProllySection.CircuitBreaker.OpenTimeInMiliseconds);
            }
        }

        public static int AllowedFailures
        {
            get
            {
                return ProllyConfiguration.ProllySection.CircuitBreaker.AllowedFailures;
            }
        }
    }
}
