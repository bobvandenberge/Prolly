using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Prolly.Exceptions;
using Prolly.Patterns;
using Prolly.Commands;

namespace Prolly.Tests.Patterns.Bulkhead
{
    [TestClass]
    public class SimpleBulkheadTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            Prolly.Reset();
        }

        [TestMethod]
        public void Positive_Compartments_Created_Object()
        {
            // Assign           
            var key = CommandGroupKey.Factory.Resolve("test");

            // Act
            AbstractBulkhead bulkhead = AbstractBulkhead.Factory.Resolve(key);

            // Assert
            Assert.IsNotNull(bulkhead);
        }

        [TestMethod]
        public void HasRoom_True_By_Default()
        {
            // Assign           
            var key = CommandGroupKey.Factory.Resolve("test");
            AbstractBulkhead bulkhead = AbstractBulkhead.Factory.Resolve(key);

            // Act
            var result = bulkhead.HasRoom;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod, Timeout(200)]
        public void CurrentlyActiveTasks_Reduces_After_Completion()
        {
            // Assign
            var key = CommandGroupKey.Factory.Resolve("test");
            AbstractBulkhead bulkhead = AbstractBulkhead.Factory.Resolve(key);
            
            // Act
            Task t = Task.Factory.StartNew(() => { });
            bulkhead.MarkExecution();
            bulkhead.MarkExecution();
            t.Wait();
            bulkhead.MarkCompletion();
            bulkhead.MarkCompletion();

            // Assert
            Assert.IsTrue(bulkhead.HasRoom);
        }

        [TestMethod]
        public void HasRoom_Returns_False_If_Max_Concurrency_Reached()
        {
            // Assign
            var key = CommandGroupKey.Factory.Resolve("test");
            AbstractBulkhead bulkhead = AbstractBulkhead.Factory.Resolve(key);

            // Act
            bulkhead.MarkExecution();
            bulkhead.MarkExecution();

            // Assert
            Assert.IsFalse(bulkhead.HasRoom);
        }

        [TestMethod]
        [ExpectedException(typeof(MaximumAllowedTasksReachedException))]
        public void TaskStarted_Throws_Exception_If_Max_Reached()
        {
            // Assign
            var key = CommandGroupKey.Factory.Resolve("test");
            AbstractBulkhead bulkhead = AbstractBulkhead.Factory.Resolve(key);

            // Act
            bulkhead.MarkExecution();
            bulkhead.MarkExecution();
            bulkhead.MarkExecution();

            // Assert
            // Expected Exception
        }
    }
}
