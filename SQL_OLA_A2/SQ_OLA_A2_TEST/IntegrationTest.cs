using SQL_OLA_A2;
using Task = SQL_OLA_A2.Task;


namespace SQ_OLA_A2_TEST;
using Microsoft.EntityFrameworkCore;

public class IntegrationTests
{
    
    //Mocking the database
    
    public IntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new ApplicationDbContext(options);
        _taskMapper = new TaskMapper(context);
        _taskFacade = new TaskFacade(context);
        //Clear the database before each test
        context.Database.EnsureDeleted();
    }
    
    private TaskMapper _taskMapper;
    private TaskFacade _taskFacade;
    
    
    
    [Fact]
    public void ShouldCreateAndPersistTask()
    {
        //Test CreateTask
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now, false, "Test");
        _taskMapper.AddTask(task);
        Task taskFromDb = _taskMapper.GetTaskById(task.Id);
        Assert.Equal("Test", taskFromDb.Description);
        Assert.Equal(now, taskFromDb.Deadline);
        Assert.False(taskFromDb.IsFinished);
        Assert.Equal("Test", taskFromDb.Category);
    }
    
    [Fact]
    public void ShouldUpdateAndPersistTask()
    {
        //Test UpdateTask
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now, false, "Test");
        _taskMapper.AddTask(task);
        Task taskFromDb = _taskMapper.GetTaskById(task.Id);
        task = _taskFacade.UpdateTask(taskFromDb, "Test2", now, true, "Test2");
        _taskMapper.UpdateTask(task);
        Task updatedTask = _taskMapper.GetTaskById(task.Id);
        Assert.Equal("Test2", updatedTask.Description);
        Assert.Equal(now, updatedTask.Deadline);
        Assert.True(updatedTask.IsFinished);
        Assert.Equal("Test2", updatedTask.Category);
    }
    
    [Fact]
    public void ShouldGetTaskById()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now, false, "Test");
        _taskFacade.SaveTask(task);
        Task taskById = _taskFacade.GetTaskById(task.Id);
        Assert.Equal(task, taskById);
    }
    
    [Fact]
    public void ShouldDeleteTask()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now, false, "Test");
        _taskFacade.SaveTask(task);
        _taskFacade.DeleteTask(task);
        Assert.Throws<KeyNotFoundException>(() => _taskFacade.GetTaskById(task.Id));
    }
    
    [Fact]
    public void ShouldGetAllTasks()
    {
        DateTime now = DateTime.Now;
        Task task1 = _taskFacade.CreateTask("Test1", now, false, "Test");
        _taskFacade.SaveTask(task1);
        Task task2 = _taskFacade.CreateTask("Test2", now, false, "Test");
        _taskFacade.SaveTask(task2);
        Task task3 = _taskFacade.CreateTask("Test3", now, false, "Test");
        _taskFacade.SaveTask(task3);
        List<Task> tasks = _taskFacade.GetAllTasks();
        Assert.Equal(3, tasks.Count);
    }
    
    [Fact]
    public void ShouldSaveTask()
    {
        DateTime now = DateTime.Now;
        Task task = _taskFacade.CreateTask("Test", now, false, "Test");
        _taskFacade.SaveTask(task);
        Task taskFromDb = _taskMapper.GetTaskById(task.Id);
        Assert.Equal("Test", taskFromDb.Description);
        Assert.Equal(now, taskFromDb.Deadline);
        Assert.False(taskFromDb.IsFinished);
        Assert.Equal("Test", taskFromDb.Category);
    }
    
    [Fact]
    public void ShouldAddTask()
    {
        DateTime now = DateTime.Now;
        int count = _taskFacade.GetAllTasks().Count;
        Task task = _taskFacade.CreateTask("Test", now, false, "Test");
        _taskFacade.AddTask("Test", now, false, "Test");
        int newCount = _taskFacade.GetAllTasks().Count;
        Assert.Equal(count + 1 , newCount);
    }
}
