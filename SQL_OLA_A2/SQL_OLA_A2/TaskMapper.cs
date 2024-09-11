namespace SQL_OLA_A2;

public class TaskMapper
{
    private ApplicationDbContext context;
    
    public TaskMapper(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    //This method creates takes a task object and creates a new task in the database
    public void AddTask(Task task)
    {
        context.Tasks.Add(task);
        context.SaveChanges();
    }
    
    //This method takes a task object and updates the task in the database
    public void UpdateTask(Task task)
    {
        context.Tasks.Update(task);
        context.SaveChanges();
    }   
    
    //This method takes a task object and deletes the task from the database
    public void DeleteTask(Task task)
    {
        context.Tasks.Remove(task);
        context.SaveChanges();
    }

    public List<Task> GetAllTasks()
    {
        return context.Tasks.ToList();
    }
    
    //This method takes an id and returns the task with that id
    public Task GetTaskById(int id)
    {
        return context.Tasks.Find(id);
    }
}