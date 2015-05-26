using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolly.Patterns;
using Prolly.Commands;

namespace Prolly.Tests
{

    [TestClass]
    public class SimpleCircuitBreakerTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            Prolly.Reset();
        }

        [TestMethod]
        public void IsOpen_Default_False()
        {
            // Arrange
            var key = CommandGroupKey.Factory.Resolve("test");
            var sut = AbstractCircuitBreaker.Factory.Resolve(key);

            // Act
            var result = !sut.AllowRequests;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryBreak_Under_Threshold_Doesnt_Brake()
        {
            // Arrange
            var key = CommandGroupKey.Factory.Resolve("test");
            var sut = AbstractCircuitBreaker.Factory.Resolve(key);

            // Act
            sut.MarkFailure();
            var result = !sut.AllowRequests;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryBreak_On_Threshold_Opens_Breaker()
        {
            // Arrange
            var key = CommandGroupKey.Factory.Resolve("test");
            var sut = AbstractCircuitBreaker.Factory.Resolve(key);

            // Act
            sut.MarkFailure();
            sut.MarkFailure();
            var result = !sut.AllowRequests;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod, Timeout(500)]
        public void Transition_HalfOpen_After_Specified_Time()
        {
            // Arrange
            var key = CommandGroupKey.Factory.Resolve("test");
            var sut = AbstractCircuitBreaker.Factory.Resolve(key);

            // Act
            sut.MarkFailure();
            sut.MarkFailure();

            // Assert
            while(!sut.AllowRequests) // Just wait for the timer to change the status to HalfOpen
            {}
        }

        [TestMethod, Timeout(500)]
        public void MarkSucces_Opens_Breaker_If_Half_Open()
        {
            // Arrange
            var key = CommandGroupKey.Factory.Resolve("test");
            var sut = AbstractCircuitBreaker.Factory.Resolve(key);

            // Act
            sut.MarkFailure();
            sut.MarkFailure();
            while ( !sut.AllowRequests ) // Just wait for the timer to change the status to HalfOpen
            { }
            sut.MarkSucces();
            var result = !sut.AllowRequests;

            // Assert
            Assert.IsFalse(result);
        }
    }
}
