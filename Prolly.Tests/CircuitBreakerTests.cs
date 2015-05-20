using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolly.Patterns;

namespace Prolly.Tests
{
    [TestClass]
    public class CircuitBreakerTests
    {
        [TestMethod]
        public void IsOpen_Default_False()
        {
            // Arrange
            var sut = new CircuitBreaker();

            // Act
            var result = sut.IsOpen;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryBreak_Under_Threshold_Doesnt_Brake()
        {
            // Arrange
            var sut = new CircuitBreaker();

            // Act
            sut.TryBreak();
            var result = sut.IsOpen;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryBreak_On_Threshold_Opens_Breaker()
        {
            // Arrange
            var sut = new CircuitBreaker();

            // Act
            sut.TryBreak();
            sut.TryBreak();
            var result = sut.IsOpen;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ForceOpen_Opens_Breaker()
        {
            // Arrange
            var sut = new CircuitBreaker();

            // Act
            sut.ForceOpen();
            var result = sut.IsOpen;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ForceClose_Closes_Breaker()
        {
            // Arrange
            var sut = new CircuitBreaker();

            // Act
            sut.ForceClose();
            var result = sut.IsOpen;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod, Timeout(500)]
        public void Transition_HalfOpen_After_Specified_Time()
        {
            // Arrange
            var sut = new CircuitBreaker(2, TimeSpan.FromMilliseconds(0));

            // Act
            sut.TryBreak();
            sut.TryBreak();

            // Assert
            while(!sut.AllowRequest) // Just wait for the timer to change the status to HalfOpen
            {}
        }

        [TestMethod, Timeout(500)]
        public void MarkSucces_Opens_Breaker_If_Half_Open()
        {
            // Arrange
            var sut = new CircuitBreaker(2, TimeSpan.FromMilliseconds(0));

            // Act
            sut.TryBreak();
            sut.TryBreak();
            while ( !sut.AllowRequest ) // Just wait for the timer to change the status to HalfOpen
            { }
            sut.MarkSucces();
            var result = sut.IsOpen;

            // Assert
            Assert.IsFalse(result);
        }
    }
}
