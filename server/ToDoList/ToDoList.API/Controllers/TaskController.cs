using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.API.Models;
using ToDoList.API.Repositories;

namespace ToDoList.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IRepository<TaskItem> _taskRepository;

        public TaskController(IRepository<TaskItem> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Helper method to get UserId from JWT
        /// </summary>
        /// <returns></returns>
        private Guid GetUserId()
        {
            var userIdString = User.FindFirst("UserId")?.Value;
            return Guid.Parse(userIdString);
        }

        /// <summary>
        /// Create Task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            task.UserId = GetUserId();
            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            return Ok(task);
        }

        /// <summary>
        /// Get Tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userId = GetUserId();
            var tasks = await _taskRepository.FindAsync(t => t.UserId == userId);
            return Ok(tasks);
        }

        /// <summary>
        /// Update Task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedTask"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskItem updatedTask)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null || task.UserId != userId)
                return NotFound();

            // Update properties
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.IsCompleted = updatedTask.IsCompleted;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();

            return Ok(task);
        }

        /// <summary>
        /// Update IsCompleted Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isCompleted"></param>
        /// <returns></returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] bool isCompleted)
        {
            var userId = GetUserId();
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null || task.UserId != userId)
                return NotFound();

            task.IsCompleted = isCompleted;
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();

            return Ok(task);
        }

        /// <summary>
        /// Delete Task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = GetUserId();
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null || task.UserId != userId)
                return NotFound();

            _taskRepository.Delete(task);
            await _taskRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
