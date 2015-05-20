using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Configuration
{
    public static class TimeoutConfiguration
    {
        public static TimeSpan WaitingTime
        {
            get
            {
                return TimeSpan.FromMilliseconds(ProllyConfiguration.ProllySection.Timeout.Miliseconds);
            }
        }
    }
}
