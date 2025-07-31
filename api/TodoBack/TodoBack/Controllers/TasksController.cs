using Microsoft.AspNetCore.Mvc;
using TodoBack.Models;
using TodoBack.Services;

namespace TodoApi.Controllers;

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
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _service.GetByIdAsync(id);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        var created = await _service.CreateAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = created.TaskItemId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskItem task)
    {
        if (id != task.TaskItemId)  
        {
            return BadRequest("Task ID mismatch.");
        }
        var updated = await _service.UpdateAsync(task);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("{id}/tags")]
    public async Task<IActionResult> AssignTags(int id, [FromBody] List<int> tagIds)
    {
        var success = await _service.AssignTagsAsync(id, tagIds);
        return success ? NoContent() : NotFound();
    }
}
