namespace SQL_OLA_A2;

public class Task
{
    public Task(string description, DateTime deadline, bool isCompleted, string category)
    {
        Description = description;
        Deadline = deadline;
        IsFinished = isCompleted;
        Category = category;
    }

    public Task(int id, string description, string category, DateTime deadline, bool isFinished)
    {
        Id = id;
        Description = description;
        Category = category;
        Deadline = deadline;
        IsFinished = isFinished;
    }

    public int Id { get; set; } // primary key
    public string Description { get; set; }
    public string Category { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsFinished { get; set; }
    
}