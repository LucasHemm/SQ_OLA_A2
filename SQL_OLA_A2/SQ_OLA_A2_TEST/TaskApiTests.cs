using System.Net;
using System.Net.Http.Json;
using SQL_OLA_A2;

namespace SQ_OLA_A2_TEST
{
    public class TaskApiTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TaskApiTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void GetAllTasks_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/task");

            // Assert
            response.EnsureSuccessStatusCode(); // Check if status code is 200 OK
        }   
        
        [Fact]
        public async void GetTaskById_ReturnsTask_WhenIdExists()
        {
            // Arrange: Create a new task
            var newTask = new TaskDto
            {
                Description = "Test Task",
                Deadline = DateTime.Now.AddDays(7),
                IsFinished = false,
                Category = "Test Category"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/task", newTask);
            createResponse.EnsureSuccessStatusCode();
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.NotEqual(0, createdTask.Id); // Ensure task has a valid ID

            // Act: Send GET request to retrieve the task by its ID
            var response = await _client.GetAsync($"/api/task/{createdTask.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var task = await response.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(task);
            Assert.Equal(createdTask.Id, task.Id); // Check if the task IDs match
            Assert.Equal(newTask.Description, task.Description); // Check if the description matches
        }
        
        [Fact]
        public async void CreateTask_ReturnsCreatedTask()
        {
            // Arrange
            var newTask = new TaskDto
            {
                Description = "Test Task",
                Deadline = DateTime.Now.AddDays(7),
                IsFinished = false,
                Category = "Test Category"
            };
        
            // Act
            var response = await _client.PostAsJsonAsync("/api/task", newTask);
        
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
            var createdTask = await response.Content.ReadFromJsonAsync<TaskDto>();
            Assert.Equal("Test Task", createdTask.Description);
        }
        
        [Fact]
        public async void UpdateTask_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange: Create a new task
            var newTask = new TaskDto
            {
                Description = "Initial Task",
                Deadline = DateTime.Now.AddDays(7),
                IsFinished = false,
                Category = "Initial Category"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/task", newTask);
            createResponse.EnsureSuccessStatusCode();
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.NotEqual(0, createdTask.Id); // Ensure task has a valid ID

            // Prepare update DTO
            var updatedTask = new TaskDto
            {
                Description = "Updated Task",
                Deadline = DateTime.Now.AddDays(10),
                IsFinished = true,
                Category = "Updated Category"
            };

            // Act: Send PUT request to update the task
            var response = await _client.PutAsJsonAsync($"/api/task/{createdTask.Id}", updatedTask);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Retrieve the updated task
            var getResponse = await _client.GetAsync($"/api/task/{createdTask.Id}");
            var task = await getResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(task);
            Assert.Equal(updatedTask.Description, task.Description); // Check if the description matches
            Assert.Equal(updatedTask.Deadline, task.Deadline); // Check if the deadline matches
            Assert.Equal(updatedTask.IsFinished, task.IsFinished); // Check if the IsFinished matches
            Assert.Equal(updatedTask.Category, task.Category); // Check if the category matches
        }

        [Fact]
        public async void MarkTaskAsFinished_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange: Create a new task
            var newTask = new TaskDto
            {
                Description = "Task to Finish",
                Deadline = DateTime.Now.AddDays(7),
                IsFinished = false,
                Category = "Category"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/task", newTask);
            createResponse.EnsureSuccessStatusCode();
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.NotEqual(0, createdTask.Id); // Ensure task has a valid ID

            // Act: Send PUT request to mark the task as finished
            var response = await _client.PutAsync($"/api/task/{createdTask.Id}/finish", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Retrieve the updated task
            var getResponse = await _client.GetAsync($"/api/task/{createdTask.Id}");
            var task = await getResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(task);
            Assert.True(task.IsFinished); // Check if the task is marked as finished
        }

        [Fact]
        public async void MarkTaskAsUnfinished_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange: Create a new task
            var newTask = new TaskDto
            {
                Description = "Task to Unfinish",
                Deadline = DateTime.Now.AddDays(7),
                IsFinished = true,
                Category = "Category"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/task", newTask);
            createResponse.EnsureSuccessStatusCode();
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.NotEqual(0, createdTask.Id); // Ensure task has a valid ID

            // Act: Send PUT request to mark the task as unfinished
            var response = await _client.PutAsync($"/api/task/{createdTask.Id}/unfinished", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Retrieve the updated task
            var getResponse = await _client.GetAsync($"/api/task/{createdTask.Id}");
            var task = await getResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(task);
            Assert.False(task.IsFinished); // Check if the task is marked as unfinished
        }

        [Fact]
        public async void DeleteTask_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange: Create a new task
            var newTask = new TaskDto
            {
                Description = "Task to Delete",
                Deadline = DateTime.Now.AddDays(7),
                IsFinished = false,
                Category = "Category"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/task", newTask);
            createResponse.EnsureSuccessStatusCode();
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.NotEqual(0, createdTask.Id); // Ensure task has a valid ID

            // Act: Send DELETE request to remove the task
            var response = await _client.DeleteAsync($"/api/task/{createdTask.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void IsTaskOverdue_ReturnsTrue_WhenTaskIsOverdue()
        {
            // Arrange: Create a new overdue task
            var newTask = new TaskDto
            {
                Description = "Overdue Task",
                Deadline = DateTime.Now.AddDays(-1), // Deadline in the past
                IsFinished = false,
                Category = "Category"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/task", newTask);
            createResponse.EnsureSuccessStatusCode();
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.NotEqual(0, createdTask.Id); // Ensure task has a valid ID

            // Act: Send GET request to check if the task is overdue
            var response = await _client.GetAsync($"/api/task/{createdTask.Id}/overdue");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var isOverdue = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(isOverdue); // The task should be overdue
        }

        [Fact]
        public async void IsTaskOverdue_ReturnsFalse_WhenTaskIsNotOverdue()
        {
            // Arrange: Create a new task that is not overdue
            var newTask = new TaskDto
            {
                Description = "Not Overdue Task",
                Deadline = DateTime.Now.AddDays(1), // Deadline in the future
                IsFinished = false,
                Category = "Category"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/task", newTask);
            createResponse.EnsureSuccessStatusCode();
            var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.NotEqual(0, createdTask.Id); // Ensure task has a valid ID

            // Act: Send GET request to check if the task is overdue
            var response = await _client.GetAsync($"/api/task/{createdTask.Id}/overdue");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var isOverdue = await response.Content.ReadFromJsonAsync<bool>();
            Assert.False(isOverdue); // The task should not be overdue
        }
    }
}
