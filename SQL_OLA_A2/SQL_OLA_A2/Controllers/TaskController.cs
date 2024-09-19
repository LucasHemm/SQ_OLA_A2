using Microsoft.AspNetCore.Mvc;


namespace SQL_OLA_A2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskFacade _taskFacade;

        // Constructor injection of TaskFacade
        public TaskController(ApplicationDbContext context)
        {
            _taskFacade = new TaskFacade(context);
        }

        // GET: api/task
        [HttpGet]
        public ActionResult<List<Task>> GetAllTasks()
        {
            var tasks = _taskFacade.GetAllTasks();
            return Ok(tasks);
        }

        // GET: api/task/{id}
        [HttpGet("{id}")]
        public ActionResult<Task> GetTaskById(int id)
        {
            var task = _taskFacade.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // POST: api/task
        [HttpPost]
        public ActionResult<Task> CreateTask([FromBody] TaskDto taskDto)
        {
            var task = _taskFacade.AddTask(taskDto.Description, taskDto.Deadline, taskDto.IsFinished, taskDto.Category);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // PUT: api/task/{id}
        [HttpPut("{id}")]
        public ActionResult<Task> UpdateTask(int id, [FromBody] TaskDto taskDto)
        {
            var task = _taskFacade.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            _taskFacade.UpdateTask(task, taskDto.Description, taskDto.Deadline, taskDto.IsFinished, taskDto.Category);
            _taskFacade.SaveTask(task);
            return NoContent();
        }

        // PUT: api/task/{id}/finish
        [HttpPut("{id}/finish")]
        public ActionResult MarkTaskAsFinished(int id)
        {
            var task = _taskFacade.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            _taskFacade.MarkAsFinished(task);
            _taskFacade.SaveTask(task);
            return NoContent();
        }

        // PUT: api/task/{id}/unfinished
        [HttpPut("{id}/unfinished")]
        public ActionResult MarkTaskAsUnfinished(int id)
        {
            var task = _taskFacade.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            _taskFacade.MarkAsUnfinished(task);
            _taskFacade.SaveTask(task);
            return NoContent();
        }

        // DELETE: api/task/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTask(int id)
        {
            var task = _taskFacade.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            _taskFacade.DeleteTask(task);
            return NoContent();
        }

        // GET: api/task/{id}/overdue
        [HttpGet("{id}/overdue")]
        public ActionResult<bool> IsTaskOverdue(int id)
        {
            var task = _taskFacade.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(_taskFacade.IsOverdue(task));
        }
    }
}

