namespace SQL_OLA_A2;

public class TaskFacade
{
    
    
    //constructor
    public TaskFacade(ApplicationDbContext context)
    {
        taskMapper = new TaskMapper(context);
    }
    
    TaskMapper taskMapper;
    //Create a Task
    public Task CreateTask( string description, DateTime deadline, bool IsFinished, string category)
    {
        Task task = new Task
        ( description, deadline, IsFinished, category);
        return task;
    }
    
    //create and add a task
    public Task AddTask( string description, DateTime deadline, bool IsFinished, string category)
    {
        Task task = CreateTask(description, deadline, IsFinished, category);
        taskMapper.AddTask(task);
        return task;
    }
    
    
    //Update a Task
    public Task UpdateTask(Task task, string description, DateTime deadline, bool IsFinished, string category)
    {
        task.Description = description;
        task.Deadline = deadline;
        task.IsFinished = IsFinished;
        task.Category = category;
        return task;
    }
    
    //Update deadline
    public Task UpdateDeadline(Task task, DateTime deadline)
    {
        task.Deadline = deadline;
        return task;
    }
    
    //Get all tasks
    public List<Task> GetAllTasks()
    {
        return taskMapper.GetAllTasks();
    }
    
    //Get a task by id
    public Task GetTaskById(int id)
    {
        return taskMapper.GetTaskById(id);
    }
    
    //Mark a task as finished
    public Task MarkAsFinished(Task task)
    {
        task.IsFinished = true;
        return task;
    }
    
    public Task MarkAsUnfinished(Task task)
    {
        task.IsFinished = false;
        return task;
    }
    
    // Save updated task
    public void SaveTask(Task task)
    {
        taskMapper.UpdateTask(task);
    }
    
    //Delete a Task
    public void DeleteTask(Task task)
    {
        taskMapper.DeleteTask(task);
    }
    
    //Checks if task is overdue
    public bool IsOverdue(Task task)
    {
        return task.Deadline < DateTime.Now && !task.IsFinished;
    }
}