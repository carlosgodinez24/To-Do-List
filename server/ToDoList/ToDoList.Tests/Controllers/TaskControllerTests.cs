using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using ToDoList.API.Controllers;
using ToDoList.API.Models;
using ToDoList.API.Repositories;

namespace ToDoList.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<IRepository<TaskItem>> _taskRepositoryMock;
        private readonly TaskController _taskController;
        private static Guid _testUserId = new("6D2638DE-522C-4CD5-8B26-1C0E95B3B479");

        public TaskControllerTests()
        {
            _taskRepositoryMock = new Mock<IRepository<TaskItem>>();

            // Set up the controller with a mock user
            _taskController = new TaskController(_taskRepositoryMock.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new("UserId", _testUserId.ToString())
            }, "mock"));

            _taskController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetTasks_ReturnsTasksForUser()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new() { TaskId = Guid.NewGuid(), Title = "Task 1", UserId = _testUserId },
                new() { TaskId = Guid.NewGuid(), Title = "Task 2", UserId = _testUserId }
            };

            _taskRepositoryMock.Setup(repo => repo.FindAsync(t => t.UserId == _testUserId))
                .ReturnsAsync(tasks);


            // Act
            var result = await _taskController.GetTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(okResult.Value);
            Assert.Equal(2, ((List<TaskItem>)returnedTasks).Count);
        }

        [Fact]
        public async Task CreateTask_ValidModel_ReturnsCreatedTask()
        {
            // Arrange
            var newTask = new TaskItem { Title = "New Task", Description = "Test Description" };

            _taskRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<TaskItem>()))
                .Returns(Task.CompletedTask);

            _taskRepositoryMock.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.CreateTask(newTask);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var createdTask = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal("New Task", createdTask.Title);
            Assert.Equal(_testUserId, createdTask.UserId);
        }

        [Fact]
        public async Task UpdateTask_TaskExists_ReturnsUpdatedTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var existingTask = new TaskItem { TaskId = taskId, UserId = _testUserId, Title = "Old Title" };
            var updatedTask = new TaskItem { Title = "Updated Title", Description = "Updated Description", DueDate = DateTime.Now, IsCompleted = true };

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync(existingTask);

            _taskRepositoryMock.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.UpdateTask(taskId, updatedTask);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var task = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal("Updated Title", task.Title);
        }

        [Fact]
        public async Task DeleteTask_TaskExists_ReturnsNoContent()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var existingTask = new TaskItem { TaskId = taskId, UserId = _testUserId };

            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync(existingTask);

            _taskRepositoryMock.Setup(repo => repo.Delete(existingTask));
            _taskRepositoryMock.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskController.DeleteTask(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
