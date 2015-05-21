using Prolly.Configuration.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolly.Configuration
{
    /// <summary>
    /// Prolly Configuration
    /// </summary>
    internal static class ProllyConfiguration
    {
        internal static ProllySection ProllySection = (ProllySection) System.Configuration.ConfigurationManager.GetSection("prollySettings/prolly");
    }
}
