using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolly.Tests.TestSupport;
using Prolly.Exceptions;

namespace Prolly.Tests
{
    [TestClass]
    public class ProllyCommandTests
    {
        [TestMethod]
        public void Execute_Returns_Run_Value()
        {
            // Arrange
            var sut = new SuccesCommand();

            // Act
            string result = sut.Execute();

            // Assert
            Assert.AreEqual(SuccesCommand.ReturnValue, result);
        }

        [TestMethod]
        public void ExecuteAsync_Returns_Run_Value()
        {
            // Arrange
            var sut = new SuccesCommand();

            // Act
            string result = sut.ExecuteAsync().Result;

            // Assert
            Assert.AreEqual(SuccesCommand.ReturnValue, result);
        }

        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void Execute_Throws_Exception_On_Timeout_Default_1_Second()
        {
            // Arrange
            var sut = new TimeoutCommand(TimeSpan.FromMilliseconds(400));

            // Act
            string result = sut.Execute();

            // Assert
            Assert.AreEqual(TimeoutCommand.ReturnValue, result);
        }

        [TestMethod]
        public void Execute_Returns_Fallback_After_Timeout()
        {
            // Arrange
            var sut = new TimeoutCommandWithFallback(TimeSpan.FromMilliseconds(400));

            // Act
            string result = sut.Execute();

            // Assert
            Assert.AreEqual(TimeoutCommandWithFallback.FallbackValue, result);
        }

        [TestMethod]
        [ExpectedException(typeof(CircuitBreakerOpenException))]
        public void Execute_No_Fallback_Trips_CircuitBreaker_After_Two_Fails()
        {
            // Arrange
            var sut = new TimeoutCommand(TimeSpan.FromMilliseconds(400));

            // Act
            try { string result = sut.Execute(); }
            catch ( Exception ) { }

            try { string result = sut.Execute(); }
            catch ( Exception ) { }

            sut.Execute();

            // Assert
            // ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(CircuitBreakerOpenException))]
        public void Execute_Different_Instance_Same_Command_Break_Ciruit()
        {
            // Arrange
            var timeoutCommand1 = new TimeoutCommand(TimeSpan.FromMilliseconds(400));
            var timeoutCommand2 = new TimeoutCommand(TimeSpan.FromMilliseconds(400));
            var timeoutCommand3 = new TimeoutCommand(TimeSpan.FromMilliseconds(400));

            // Act
            try { string result = timeoutCommand1.Execute(); }
            catch ( Exception ) { }

            try { string result = timeoutCommand2.Execute(); }
            catch ( Exception ) { }

            timeoutCommand3.Execute();

            // Assert
            // ExpectedException
        }
    }
}
