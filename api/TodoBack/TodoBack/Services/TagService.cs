using Microsoft.EntityFrameworkCore;
using TodoBack.Data;
using TodoBack.Models;

namespace TodoBack.Services
{
    public class TagService: ITagService
    {
        private readonly AppDbContext _context;
        public TagService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }
        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }
        public async Task<Tag> CreateAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }
        public async Task<Tag> UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return false;
            }
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<List<Tag>> GetTagsByTaskIdAsync(int taskId)
        {
            return await _context.TaskTags
                .Where(tt => tt.TaskItemId == taskId)
                .Select(tt => tt.Tag)
                .ToListAsync();
        }
    }
}
