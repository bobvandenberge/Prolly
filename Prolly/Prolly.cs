using Prolly.Commands;
using Prolly.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prolly
{
    /// <summary>
    /// Util class for Prolly things
    /// </summary>
    public static class Prolly
    {
        /// <summary>
        /// Reset all Circuit breakers, bulkhead, command groups etc.
        /// </summary>
        public static void Reset()
        {
            CommandGroupKey.Factory.Reset();
            CommandKey.Factory.Reset();
            AbstractBulkhead.Factory.Reset();
            AbstractCircuitBreaker.Factory.Reset();
            AbstractTimeout.Factory.Reset();
        }
    }
}
