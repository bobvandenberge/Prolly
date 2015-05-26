using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolly.Patterns;
using Prolly.Configuration;

namespace Prolly.Tests.Configuration
{
    [TestClass]
    public class SectionsTests
    {
        [TestMethod]
        public void TimeoutConfiguration_WaitingTime_Loaded_From_Configuration()
        {
            // Arrange
            // Act
            var result = TimeoutConfiguration.WaitingTime;

            // Assert
            Assert.AreEqual(1000, (int) result.TotalMilliseconds);
        }

        [TestMethod]
        public void CircuitBreakerConfiguration_AllowedFailures_Loaded_From_Configuration()
        {
            // Arrange
            // Act
            int result = CircuitBreakerConfiguration.AllowedFailures;

            // Assert
            Assert.AreEqual(2, result);
        }
        
        [TestMethod]
        public void CircuitBreakerConfiguration_TimeOpen_Loaded_From_Configuration()
        {
            // Arrange

            // Act
            int result = (int) CircuitBreakerConfiguration.TimeOpen.TotalMilliseconds;

            // Assert
            Assert.AreEqual(200, result);
        }

        [TestMethod]
        public void BulkheadConfiguration_ConcurrentTasks_Loaded_From_Configuration()
        {
            // Arrange
            // Act
            int result = (int) BulkheadConfiguration.ConcurrentTasks;

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}
