using Moq;
using Xunit;
using System;
using Microsoft.EntityFrameworkCore;
using SQL_OLA_A2;
using Task = SQL_OLA_A2.Task;

namespace SQ_OLA_A2_TEST
{
    public class UnitTests
    {
        private TaskFacade _taskFacade;

        public UnitTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new ApplicationDbContext(options);
            _taskFacade = new TaskFacade(context);
        }

        [Fact]
        public void ShouldCreateTask()
        {
            // Test CreateTask
            DateTime now = DateTime.Now;
            Task task = _taskFacade.CreateTask("Test", now, false, "Test");
            Assert.Equal("Test", task.Description);
            Assert.Equal(now, task.Deadline);
            Assert.False(task.IsFinished);
            Assert.Equal("Test", task.Category);
        }

        [Fact]
        public void ShouldUpdateTask()
        {
            // Test UpdateTask
            DateTime now = DateTime.Now;

            // Create a mock Task
            var mockTask = new Mock<Task>("Test", now, false, "Test");
            mockTask.SetupAllProperties();

            var updatedTask = _taskFacade.UpdateTask(mockTask.Object, "Test2", now, true, "Test2");

            Assert.Equal("Test2", updatedTask.Description);
            Assert.Equal(now, updatedTask.Deadline);
            Assert.True(updatedTask.IsFinished);
            Assert.Equal("Test2", updatedTask.Category);
        }

        [Fact]
        public void ShouldMarkTaskAsFinished()
        {
            // Test MarkAsFinished
            DateTime now = DateTime.Now;
            Task task = _taskFacade.CreateTask("Test", now, false, "Test");
            task = _taskFacade.MarkAsFinished(task);
            Assert.True(task.IsFinished);
        }

        [Fact]
        public void ShouldMarkTaskAsUnfinished()
        {
            // Test MarkAsUnfinished
            DateTime now = DateTime.Now;
            Task task = _taskFacade.CreateTask("Test", now, true, "Test");
            task = _taskFacade.MarkAsUnfinished(task);
            Assert.False(task.IsFinished);
        }

        [Fact]
        public void ShouldUpdateDeadline()
        {
            DateTime now = DateTime.Now;
            var mockTask = new Mock<Task>("DummyTask", now, false, "Dummy task description");
            mockTask.SetupAllProperties();
            DateTime newDate = now.AddDays(1);

            var updatedTask = _taskFacade.UpdateDeadline(mockTask.Object, newDate);
            Assert.Equal(newDate, updatedTask.Deadline);
        }
    }
}
