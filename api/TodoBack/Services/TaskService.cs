using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoBack.Data;
using TodoBack.Models;
using TodoBack.Models.DTO;
namespace TodoBack.Services
{
    public class TaskService: ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISubtaskService _subtaskService;
        private readonly ITagService _tagService;

        public TaskService(AppDbContext context, IMapper mapper, ISubtaskService subtaskService, ITagService tagService)
        {
            _context = context;
            _mapper =mapper;
            _subtaskService = subtaskService;
            _tagService = tagService;
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

        public async Task<List<TaskDto>> GetAllTodayAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var taskItems = await _context.Tasks
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Include(t => t.Subtasks)
                .Where(t => t.TaskDueDate >= today && t.TaskDueDate < tomorrow)
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

        public async Task<TaskDto> CreateAsync(TaskCreateDto taskDto)
        {
            var taskItem = _mapper.Map<TaskItem>(taskDto);
            _context.Tasks.Add(taskItem);
            var itemCreated = await _context.SaveChangesAsync();

            var subtasksCreated=await _subtaskService.CreateManyAsync(taskDto.Subtasks, taskItem);
            var tagsAssigned = await _tagService.AssignTagsAsync(taskDto.Tags, taskItem);

            var response = _mapper.Map<TaskDto>(taskItem);
            response.Tags = tagsAssigned;
            response.Subtasks = subtasksCreated;
            return response;
        }

        public async Task<TaskDto?> UpdateAsync(int id, TaskCreateDto taskDto)
        {
            var task = await _context.Tasks
                .Include(t => t.Subtasks)
                .Include(t => t.TaskTags)
                .FirstOrDefaultAsync(t => t.TaskItemId == id);

            if (task == null) return null;

            var subtasks = await _subtaskService.SyncSubtasksAsync(taskDto.Subtasks, task);
            var tags = await _tagService.SyncTagsAsync(taskDto.Tags, task);

            _mapper.Map(taskDto, task);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<TaskDto>(task);
            response.Tags = tags;
            response.Subtasks= subtasks;
            return response;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var taskItem = await _context.Tasks
                .Include(t => t.TaskTags)
                .Include(t => t.Subtasks)
                .FirstOrDefaultAsync(t => t.TaskItemId == id);

            if (taskItem == null)
            {
                return false;
            }

            _context.TaskTags.RemoveRange(taskItem.TaskTags);
            _context.Subtasks.RemoveRange(taskItem.Subtasks);
            _context.Tasks.Remove(taskItem);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<TaskItem?> CompleteAsync(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null)
            {
                return null;
            }
            taskItem.TaskCompleted = !taskItem.TaskCompleted;
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<bool> AssignTagsAsync(int taskId, List<int> tagIds)
        {
            var taskItem = await _context.Tasks.Include(t => t.TaskTags).FirstOrDefaultAsync(t => t.TaskItemId == taskId);
            if (taskItem == null)
            {
                return false;
            }
            taskItem.TaskTags.Clear();
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
