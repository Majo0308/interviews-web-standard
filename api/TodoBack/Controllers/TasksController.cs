using Microsoft.AspNetCore.Mvc;
using TodoBack.Models;
using TodoBack.Models.DTO;
using TodoBack.Services;

namespace TodoBack.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskDto>>> GetAll()
        {
            var tasks = await _service.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            var task = await _service.GetByIdAsync(id);
            return task is null ? NotFound() : Ok(task);
        }
        [HttpGet("by-tags/{id}")]
        public async Task<ActionResult<List<TaskDto>>> GetByTagId(int id)
        {
            var tasks = await _service.GetTaskByTagId(id);
            return tasks is null ? NotFound() : Ok(tasks);
        }
        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create(TaskCreateDto task)
        {
            var created = await _service.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = created.TaskItemId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto>> Update(int id, TaskCreateDto task)
        {
            if (id != task.TaskItemId)
                return BadRequest("Task ID mismatch.");

            var updated = await _service.UpdateAsync(id, task);
            return updated is null ? NotFound() : Ok(updated);
        }
        [HttpGet("completed/{id}")]
        public async Task<ActionResult<TaskItem>> Complete(int id)
        {

            var updated = await _service.CompleteAsync(id);
            return updated is null ? NotFound() : Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? id : NotFound();
        }

        [HttpPost("{id}/tags")]
        public async Task<ActionResult<bool>> AssignTags(int id, [FromBody] List<int> tagIds)
        {
            var success = await _service.AssignTagsAsync(id, tagIds);
            return success ? success : NotFound();
        }
    }
}
