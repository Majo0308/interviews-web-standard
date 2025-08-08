using Microsoft.AspNetCore.Mvc;
using TodoBack.Models;
using TodoBack.Models.DTO;
using TodoBack.Services;

namespace TodoBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubtasksController : Controller
    {
        private readonly ISubtaskService _subtaskService;
        public SubtasksController(ISubtaskService subtaskService)
        {
            _subtaskService = subtaskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubtaskStateDto>>> GetAll()
        {
            var subtasks = await _subtaskService.GetAllAsync();
            return Ok(subtasks);
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<SubtaskDto>> GetById(int id)
        {
            var subtask = await _subtaskService.GetByIdAsync(id);
            if (subtask == null)
            {
                return NotFound();
            }
            return Ok(subtask);
        }
        [HttpPost] 
        public async Task<ActionResult<SubtaskDto>> Create(SubtaskDto subtask)
        {
            var created = await _subtaskService.CreateAsync(subtask);
            return created;
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<SubtaskDto>> Update(int id, SubtaskDto subtask)
        {
            var updated = await _subtaskService.UpdateAsync(id, subtask);
            return Ok(updated);
        }
        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>> Delete(int id)
        {
            var success = await _subtaskService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }


    }
}
