using Microsoft.AspNetCore.Mvc;
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("{id}/tags")]
        public async Task<ActionResult> AssignTags(int id, [FromBody] List<int> tagIds)
        {
            var success = await _service.AssignTagsAsync(id, tagIds);
            return success ? NoContent() : NotFound();
        }
    }
}
