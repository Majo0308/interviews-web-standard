using TodoBack.Data;
using Microsoft.EntityFrameworkCore;
using TodoBack.Models;
using TodoBack.Models.DTO;
using AutoMapper;
namespace TodoBack.Services
{
    public class TaskService: ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TaskService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper =mapper;
        }   
        public async Task<List<TaskDto>> GetAllAsync()
        {
           var taskItems = await _context.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Include(t => t.Subtasks)
                .ToListAsync();
            return _mapper.Map<List<TaskDto>>(taskItems);


        }
        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            var taskItem = await _context.Tasks
               .Include(t => t.TaskTags)
                   .ThenInclude(tt => tt.Tag)
               .Include(t => t.Subtasks)
               .FirstOrDefaultAsync(t => t.TaskItemId == id);
            return taskItem == null ? null : _mapper.Map<TaskDto>(taskItem);
        }
        public async Task<List<TaskDto>> GetTaskByTagId(int id)
        {
            var tasks = await _context.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Include(t => t.Subtasks)
                .Where(t => t.TaskTags.Any(tt => tt.TagId == id))
                .ToListAsync();

            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskItem> CreateAsync(TaskCreateDto taskDto)
        {
            var taskItem = _mapper.Map<TaskItem>(taskDto);
            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();
            if (taskDto.Tags?.Any() == true)
            {
                taskItem.TaskTags = taskDto.Tags
                    .Select(tagId => new TaskTag { TaskItemId = taskItem.TaskItemId, TagId = tagId })
                    .ToList();

                await _context.SaveChangesAsync();
            }
            return taskItem;
        }

        public async Task<TaskItem?> UpdateAsync(int id, TaskCreateDto taskDto)
        {
            var task = await _context.Tasks
                .Include(t => t.Subtasks)
                .Include(t => t.TaskTags)
                .FirstOrDefaultAsync(t => t.TaskItemId == id);

            if (task == null) return null;

            _mapper.Map(taskDto, task);
            await _context.SaveChangesAsync();

            return task;
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
