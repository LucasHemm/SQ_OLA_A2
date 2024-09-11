using SQL_OLA_A2;
using Task = SQL_OLA_A2.Task;

namespace SQ_OLA_A2_TEST;
using Microsoft.EntityFrameworkCore;

public class UnitTests
{
    
    private TaskFacade taskFacade;

    public UnitTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new ApplicationDbContext(options);
        taskFacade = new TaskFacade(context);
    }
    
    [Fact]
    public void ShouldCreateTask()
    {
        //Test CreateTask
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
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
        // Create a FakeTask instance
        FakeTask task = new FakeTask("Test", now, false, "Test");
    
        task = (FakeTask) taskFacade.UpdateTask(task, "Test2", now, true, "Test2");
    
        Assert.Equal("Test2", task.Description);
        Assert.Equal(now, task.Deadline);
        Assert.True(task.IsFinished);
        Assert.Equal("Test2", task.Category);
    }


    [Fact]
    public void ShouldMarkTaskAsFinished()
    {
        //Test MarkAsFinished
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
        task = taskFacade.MarkAsFinished(task);
        Assert.True(task.IsFinished);
    }

    [Fact]
    public void ShouldMarkTaskAsUnfinished()
    {
        //Test MarkAsUnfinished
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now, true, "Test");
        task = taskFacade.MarkAsUnfinished(task);
        Assert.False(task.IsFinished);
    }

    
    private class FakeTask : Task
    {
        public FakeTask(string description, DateTime deadline, bool isCompleted, string category) 
            : base(description, deadline, isCompleted, category)
        {
        }
    }

    [Fact]
    public void ShouldUpdateDeadline()
    {
        DateTime now = DateTime.Now;
        Task fakeTask = new FakeTask("DummyTask", now, false, "Dummy task description");
        DateTime newDate = now.AddDays(1);
        fakeTask = taskFacade.UpdateDeadline(fakeTask, newDate);
        Assert.Equal(newDate, fakeTask.Deadline);
    }
}