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
        taskMapper = new TaskMapper(context);
        taskFacade = new TaskFacade(context);
        //Clear the database before each test
        context.Database.EnsureDeleted();
    }
    
    private TaskMapper taskMapper;
    private TaskFacade taskFacade;
    
    
    
    [Fact]
    public void ShouldCreateAndPersistTask()
    {
        //Test CreateTask
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
        taskMapper.AddTask(task);
        Task taskFromDb = taskMapper.GetTaskById(task.Id);
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
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
        taskMapper.AddTask(task);
        Task taskFromDb = taskMapper.GetTaskById(task.Id);
        task = taskFacade.UpdateTask(taskFromDb, "Test2", now, true, "Test2");
        taskMapper.UpdateTask(task);
        Task updatedTask = taskMapper.GetTaskById(task.Id);
        Assert.Equal("Test2", updatedTask.Description);
        Assert.Equal(now, updatedTask.Deadline);
        Assert.True(updatedTask.IsFinished);
        Assert.Equal("Test2", updatedTask.Category);
    }
    
    [Fact]
    public void ShouldGetTaskById()
    {
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
        taskFacade.SaveTask(task);
        Task taskById = taskFacade.GetTaskById(task.Id);
        Assert.Equal(task, taskById);
    }
    
    [Fact]
    public void ShouldDeleteTask()
    {
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
        taskFacade.SaveTask(task);
        taskFacade.DeleteTask(task);
        Task taskById = taskFacade.GetTaskById(task.Id);
        Assert.Null(taskById);
    }
    
    [Fact]
    public void ShouldGetAllTasks()
    {
        DateTime now = DateTime.Now;
        Task task1 = taskFacade.CreateTask("Test1", now, false, "Test");
        taskFacade.SaveTask(task1);
        Task task2 = taskFacade.CreateTask("Test2", now, false, "Test");
        taskFacade.SaveTask(task2);
        Task task3 = taskFacade.CreateTask("Test3", now, false, "Test");
        taskFacade.SaveTask(task3);
        List<Task> tasks = taskFacade.GetAllTasks();
        Assert.Equal(3, tasks.Count);
    }
    
    [Fact]
    public void ShouldSaveTask()
    {
        DateTime now = DateTime.Now;
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
        taskFacade.SaveTask(task);
        Task taskFromDb = taskMapper.GetTaskById(task.Id);
        Assert.Equal("Test", taskFromDb.Description);
        Assert.Equal(now, taskFromDb.Deadline);
        Assert.False(taskFromDb.IsFinished);
        Assert.Equal("Test", taskFromDb.Category);
    }
    
    [Fact]
    public void ShouldAddTask()
    {
        DateTime now = DateTime.Now;
        int count = taskFacade.GetAllTasks().Count;
        Task task = taskFacade.CreateTask("Test", now, false, "Test");
        taskFacade.AddTask("Test", now, false, "Test");
        int newCount = taskFacade.GetAllTasks().Count;
        Assert.Equal(count + 1 , newCount);
    }
}
