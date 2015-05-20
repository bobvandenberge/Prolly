using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolly.Patterns;

namespace Prolly.Tests.Configuration
{
    [TestClass]
    public class SectionsTests
    {
        [TestMethod]
        public void Default_Timeout_WaitingTime_Loaded_From_Configuration()
        {
            // Arrange
            var sut = new Timeout();

            // Act
            var result = sut.WaitingTime;

            // Assert
            Assert.AreEqual(200, (int) result.TotalMilliseconds);
        }

        [TestMethod]
        public void Default_CircuitBreaker_AllowedFailures_Loaded_From_Configuration()
        {
            // Arrange
            var sut = new CircuitBreaker();

            // Act
            int result = sut.AllowedFailures;

            // Assert
            Assert.AreEqual(2, result);
        }
        
        [TestMethod]
        public void Default_CircuitBreaker_TimeOpen_Loaded_From_Configuration()
        {
            // Arrange
            var sut = new CircuitBreaker();

            // Act
            int result = (int) sut.TimeOpen.TotalMilliseconds;

            // Assert
            Assert.AreEqual(200, result);
        }
    }
}
