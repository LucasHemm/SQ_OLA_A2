using Microsoft.EntityFrameworkCore;
using SQL_OLA_A2;
using Task = SQL_OLA_A2.Task;

namespace ola_sq_a1;

public class SpecificationTests
{
    private TaskFacade taskFacade;

    public SpecificationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new ApplicationDbContext(options);
        taskFacade = new TaskFacade(context);
    }
    
    [Fact]
    public void ShouldReturnTrueIfTaskIsOverdueAndNotFinished()
    {
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now.AddMinutes(-1), false, "Test");
        Assert.True(taskFacade.IsOverdue(task));
    }
    
    [Fact]
    public void ShouldReturnFalseIfTaskIsNotOverdueAndNotFinished()
    {
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now.AddMinutes(1), false, "Test");
        Assert.False(taskFacade.IsOverdue(task));
    }
    
    [Fact]
    public void ShouldReturnFalseIfTaskIsOverdueAndFinished()
    {
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now.AddMinutes(-1), true, "Test");
        Assert.False(taskFacade.IsOverdue(task));
    }
    
    [Fact]
    public void ShouldReturnFalseIfNotOverdueAndFinished()
    {
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now.AddMinutes(1), true, "Test");
        Assert.False(taskFacade.IsOverdue(task));
    }
    
    [Fact]
    public void ShouldReturnTrueIfTaskIsDueNowAndNotFinished()
    {
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
        Assert.True(taskFacade.IsOverdue(task)); 
    }

    [Fact]
    public void ShouldThrowArgumentExceptionIfDescriptionIsOver200Characters()
    {
        DateTime now = DateTime.Now;
        Assert.Throws<ArgumentException>(() => taskFacade.CreateTask(new string('a', 201), now, false, "Test"));
    }
    
    [Fact]
    public void ShouldNotThrowArgumentExceptionIfDescriptionIs200Characters()
    {
        DateTime now = DateTime.Now;
        taskFacade.CreateTask(new string('a', 200), now, false, "Test");
    }
    
    [Fact]
    public void ShouldNotThrowArgumentExceptionIfDescriptionIs1Character()
    {
        DateTime now = DateTime.Now;
        taskFacade.CreateTask("a", now, false, "Test");
    }
    
    [Fact]
    public void ShouldThrowArgumentExceptionIfDescriptionIs0Characters()
    {
        DateTime now = DateTime.Now;
        Assert.Throws<ArgumentException>(() => taskFacade.CreateTask("", now, false, "Test"));
    }
    
    [Fact]
    public void ShouldNotThrowArgumentExceptionIfCategoryIs100Characters()
    {
        DateTime now = DateTime.Now;
        taskFacade.CreateTask("Test", now, false, new string('a', 100));
    }

}