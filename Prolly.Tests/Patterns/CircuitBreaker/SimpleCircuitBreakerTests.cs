using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolly.Patterns;
using Prolly.Patterns.CircuitBreaker;

namespace Prolly.Tests
{
    [TestClass]
    public class SimpleCircuitBreakerTests
    {
        [TestMethod]
        public void IsOpen_Default_False()
        {
            // Arrange
            var sut = new SimpleCircuitBreaker();

            // Act
            var result = !sut.AllowRequests;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryBreak_Under_Threshold_Doesnt_Brake()
        {
            // Arrange
            var sut = new SimpleCircuitBreaker();

            // Act
            sut.TryBreak();
            var result = !sut.AllowRequests;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryBreak_On_Threshold_Opens_Breaker()
        {
            // Arrange
            var sut = new SimpleCircuitBreaker();

            // Act
            sut.TryBreak();
            sut.TryBreak();
            var result = !sut.AllowRequests;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod, Timeout(500)]
        public void Transition_HalfOpen_After_Specified_Time()
        {
            // Arrange
            var sut = new SimpleCircuitBreaker(2, TimeSpan.FromMilliseconds(0));

            // Act
            sut.TryBreak();
            sut.TryBreak();

            // Assert
            while(!sut.AllowRequests) // Just wait for the timer to change the status to HalfOpen
            {}
        }

        [TestMethod, Timeout(500)]
        public void MarkSucces_Opens_Breaker_If_Half_Open()
        {
            // Arrange
            var sut = new SimpleCircuitBreaker(2, TimeSpan.FromMilliseconds(0));

            // Act
            sut.TryBreak();
            sut.TryBreak();
            while ( !sut.AllowRequests ) // Just wait for the timer to change the status to HalfOpen
            { }
            sut.TryRestore();
            var result = !sut.AllowRequests;

            // Assert
            Assert.IsFalse(result);
        }
    }
}
