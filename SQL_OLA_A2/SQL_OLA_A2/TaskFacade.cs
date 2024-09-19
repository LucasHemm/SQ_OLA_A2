

namespace SQL_OLA_A2
{
    public class TaskFacade
    {
        private readonly TaskMapper taskMapper;

        // Constructor injection
        public TaskFacade(ApplicationDbContext context)
        {
            taskMapper = new TaskMapper(context);
        }

        // Create a Task with validation
        public Task CreateTask(string description, DateTime deadline, bool isFinished, string category)
        {
            ValidateTaskData(description, deadline, category);
            
            Task task = new Task(description, deadline, isFinished, category);
            return task;
        }

        // Add a new task with validation
        public Task AddTask(string description, DateTime deadline, bool isFinished, string category)
        {
            Task task = CreateTask(description, deadline, isFinished, category);
            taskMapper.AddTask(task);
            return task;
        }

        // Update an existing task with validation
        public Task UpdateTask(Task task, string description, DateTime deadline, bool isFinished, string category)
        {
            ValidateTaskData(description, deadline, category);

            task.Description = description;
            task.Deadline = deadline;
            task.IsFinished = isFinished;
            task.Category = category;

            return task;
        }

        // Update task deadline with validation
        public Task UpdateDeadline(Task task, DateTime deadline)
        {
            if (deadline == default)
            {
                throw new ArgumentException("Invalid deadline. Please provide a valid date and time.");
            }

            task.Deadline = deadline;
            return task;
        }

        // Get all tasks
        public List<Task> GetAllTasks()
        {
            return taskMapper.GetAllTasks();
        }

        // Get a task by its id
        public Task GetTaskById(int id)
        {
            var task = taskMapper.GetTaskById(id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }
            return task;
        }

        // Mark task as finished
        public Task MarkAsFinished(Task task)
        {
            task.IsFinished = true;
            return task;
        }

        // Mark task as unfinished
        public Task MarkAsUnfinished(Task task)
        {
            task.IsFinished = false;
            return task;
        }

        // Save task after modification
        public void SaveTask(Task task)
        {
            taskMapper.UpdateTask(task);
        }

        // Delete a task
        public void DeleteTask(Task task)
        {
            taskMapper.DeleteTask(task);
        }

        // Check if the task is overdue
        public bool IsOverdue(Task task)
        {
            return task.Deadline < DateTime.Now && !task.IsFinished;
        }

        // Private helper method to validate task data
        private void ValidateTaskData(string description, DateTime deadline, string category)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be null or empty.");
            }
            if (description.Length > 200)
            {
                throw new ArgumentException("Description cannot exceed 200 characters.");
            }
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category cannot be null or empty.");
            }
            if (deadline == default)
            {
                throw new ArgumentException("Invalid deadline. Please provide a valid date and time.");
            }
        }

    }
}
