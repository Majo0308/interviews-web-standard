using Microsoft.AspNetCore.Mvc;
using TodoBack.Models;
using TodoBack.Services;

namespace TodoBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tag>>> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetById(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> Create(Tag tag)
        {
            var created = await _tagService.CreateAsync(tag);
            return CreatedAtAction(nameof(GetById), new { id = created.TagId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Tag>> Update(int id, Tag tag)
        {
            if (id != tag.TagId) return BadRequest();

            var updated = await _tagService.UpdateAsync(tag);

            return updated;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var deleted = await _tagService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return deleted;
        }
    }
}
