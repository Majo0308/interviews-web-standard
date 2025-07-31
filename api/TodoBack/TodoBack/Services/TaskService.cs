using TodoBack.Data;
using Microsoft.EntityFrameworkCore;
using TodoBack.Models;
namespace TodoBack.Services
{
    public class TaskService: ITaskService
    {
        private readonly AppDbContext _context;
        public TaskService(AppDbContext context)
        {
            _context = context;
        }   
        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks.Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
                .ToListAsync();
        }
        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.TaskItemId == id);
        }

        public async Task<TaskItem> CreateAsync(TaskItem taskItem)
        {
            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }   

        public async Task<TaskItem> UpdateAsync(TaskItem taskItem)
        {
            _context.Tasks.Update(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null)
            {
                return false;
            }
            _context.Tasks.Remove(taskItem);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CompleteAsync(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null)
            {
                return false;
            }
            taskItem.TaskCompleted = !taskItem.TaskCompleted;
            _context.Tasks.Update(taskItem);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AssignTagsAsync(int taskId, List<int> tagIds)
        {
            var taskItem = await _context.Tasks.Include(t => t.TaskTags).FirstOrDefaultAsync(t => t.TaskItemId == taskId);
            if (taskItem == null)
            {
                return false;
            }
            // Clear existing tags
            taskItem.TaskTags.Clear();
            // Add new tags
            foreach (var tagId in tagIds)
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag != null)
                {
                    taskItem.TaskTags.Add(new TaskTag { TaskItemId = taskId, TagId = tagId });
                }
            }
            _context.Tasks.Update(taskItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
