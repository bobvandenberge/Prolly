using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolly.Commands;

namespace Prolly.Tests.Commands
{
    [TestClass]
    public class CommandGroupKeyTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            Prolly.Reset();
        }

        [TestMethod]
        public void Resolve_Returns_New_Instance()
        {
            // Arrange
            // Act
            var result = CommandGroupKey.Factory.Resolve("test");

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Resolve_Returns_Same_Instance()
        {
            // Arrange
            var result = CommandGroupKey.Factory.Resolve("test");

            // Act
            var result2 = CommandGroupKey.Factory.Resolve("test");

            // Assert
            Assert.AreEqual(result, result2);
        }
    }
}
