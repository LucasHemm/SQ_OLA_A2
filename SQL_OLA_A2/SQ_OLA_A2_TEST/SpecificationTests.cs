using Microsoft.EntityFrameworkCore;
using SQL_OLA_A2;
using Task = SQL_OLA_A2.Task;

namespace ola_sq_a1;

public class SpecificationTests
{
    private TaskFacade _taskFacade;

    public SpecificationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new ApplicationDbContext(options);
        _taskFacade = new TaskFacade(context);
    }
    
    [Fact]
    public void ShouldReturnTrueIfTaskIsOverdueAndNotFinished()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now.AddMinutes(-1), false, "Test");
        Assert.True(_taskFacade.IsOverdue(task));
    }
    
    [Fact]
    public void ShouldReturnFalseIfTaskIsNotOverdueAndNotFinished()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now.AddMinutes(1), false, "Test");
        Assert.False(_taskFacade.IsOverdue(task));
    }
    
    [Fact]
    public void ShouldReturnFalseIfTaskIsOverdueAndFinished()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now.AddMinutes(-1), true, "Test");
        Assert.False(_taskFacade.IsOverdue(task));
    }
    
    [Fact]
    public void ShouldReturnFalseIfNotOverdueAndFinished()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now.AddMinutes(1), true, "Test");
        Assert.False(_taskFacade.IsOverdue(task));
    }
    
    [Fact]
    public void ShouldReturnTrueIfTaskIsDueNowAndNotFinished()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now, false, "Test");
        Assert.True(_taskFacade.IsOverdue(task)); 
    }

    [Fact]
    public void ShouldCreateTaskIfDescriptionIsOver200Characters()
    {
        DateTime now = DateTime.Now;
        Assert.Throws<ArgumentException>(() => _taskFacade.CreateTask(new string('a', 201), now, false, "Test"));
    }
    
    [Fact]
    public void ShouldCreateTaskIfDescriptionIs200Characters()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask(new string('a', 200), now, false, "Test");
        Assert.Equal(new string('a', 200), task.Description);
    }
    
    [Fact]
    public void ShouldCreateTaskIfDescriptionIs1Character()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("a", now, false, "Test");
        Assert.Equal("a", task.Description);
    }
    
    [Fact]
    public void ShouldThrowArgumentExceptionIfDescriptionIs0Characters()
    {
        DateTime now = DateTime.Now;
        Assert.Throws<ArgumentException>(() => _taskFacade.CreateTask("", now, false, "Test"));
    }
    
    [Fact]
    public void ShouldCreateTaskIfCategoryIs100Characters()
    {
        DateTime now = DateTime.Now; 
        Task task = _taskFacade.CreateTask("Test", now, false, new string('a', 100));
        Assert.Equal(new string('a', 100), task.Category);
    }

}