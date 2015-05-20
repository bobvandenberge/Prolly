using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Prolly.Exceptions;
using Prolly.Patterns;

namespace Prolly.Tests
{
    [TestClass]
    public class TimeoutPolicyTests
    {
        [TestMethod]
        [ExpectedException(typeof(TaskNotStartedException))]
        public void Monitor_Throws_Exception_Not_Started_Task()
        {
            // Arrange
            var sut = new Timeout(TimeSpan.FromMilliseconds(200));

            // Act
            sut.Monitor(new Task(() => { }));      

            // Assert
            // ExpectedException
        }

        [TestMethod]
        public void Monitor_Returns_Value_If_Fast_Enough()
        {
            // Arrange
            var sut = new Timeout(TimeSpan.FromMilliseconds(200));
            var task = Task<string>.Factory.StartNew(() => {
                return "prolly";
            });

            // Act
            sut.Monitor(task);
            var result = task.Result;

            // Assert
            Assert.AreEqual("prolly", result);
        }

        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void Monitor_Throws_Exception_On_Timeout()
        {
            // Arrange
            var sut = new Timeout(TimeSpan.FromMilliseconds(200));
            var task = Task<string>.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(3000);
                return "prolly";
            });

            // Act
            sut.Monitor(task);
            string result = task.Result;

            // Assert
            // ExpectedException
        }
    }
}
