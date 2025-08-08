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

        public TaskService(AppDbContext context, IMapper mapper, ISubtaskService subtaskService)
        {
            _context = context;
            _mapper =mapper;
            _subtaskService = subtaskService;
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
            }

            if (taskDto.Subtasks?.Any() == true)
            {
                var subtasks = taskDto.Subtasks.Select(subtaskDto =>
                {
                    var stateExists = _context.SubtaskStates.Any(s => s.SubtaskStateId == subtaskDto.SubtaskStateId);
                    if (!stateExists)
                        throw new ArgumentException("SubtaskState not found.");

                    var subtask = _mapper.Map<Subtask>(subtaskDto);
                    subtask.TaskItemId = taskItem.TaskItemId;
                    return subtask;
                }).ToList();

                _context.Subtasks.AddRange(subtasks);
            }

            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<TaskItem?> UpdateAsync(int id, TaskCreateDto taskDto)
        {
            var task = await _context.Tasks
                .Include(t => t.Subtasks)
                .Include(t => t.TaskTags)
                .FirstOrDefaultAsync(t => t.TaskItemId == id);

            if (task == null) return null;

            // Update TaskItem fields (conditional mapping)
            _mapper.Map(taskDto, task);

            // Synchronize Tags
            await SyncTagsAsync(task, taskDto.Tags ?? new List<int>());

            // Synchronize Subtasks
            await SyncSubtasksAsync(task, taskDto.Subtasks ?? new List<SubtaskDto>());

            await _context.SaveChangesAsync();

            return task;
        }

        private async Task SyncTagsAsync(TaskItem task, List<int> newTagIds)
        {
            var currentTagIds = task.TaskTags.Select(tt => tt.TagId).ToList();

            // Tags to add
            var tagsToAdd = newTagIds.Except(currentTagIds)
                .Select(tagId => new TaskTag { TaskItemId = task.TaskItemId, TagId = tagId });
            await _context.TaskTags.AddRangeAsync(tagsToAdd);

            // Tags to remove
            var tagsToRemove = task.TaskTags
                .Where(tt => !newTagIds.Contains(tt.TagId))
                .ToList();
            _context.TaskTags.RemoveRange(tagsToRemove);
        }

        private async Task SyncSubtasksAsync(TaskItem task, List<SubtaskDto> newSubtasks)
        {
            var existingSubtasks = task.Subtasks.ToList();

            foreach (var subtaskDto in newSubtasks)
            {
                if (subtaskDto.SubtaskId == 0)
                {
                    // New task - use service to create
                    subtaskDto.TaskItemId = task.TaskItemId;
                    await _subtaskService.CreateAsync(subtaskDto);
                }
                else
                {
                    // Update task - use service to update
                    await _subtaskService.UpdateAsync(subtaskDto.SubtaskId, subtaskDto);
                }
            }

            // Delete subtasks that are not in the incoming list
            var dtoIds = newSubtasks.Select(s => s.SubtaskId).ToList();
            var subtasksToRemove = existingSubtasks
                .Where(s => !dtoIds.Contains(s.SubtaskId))
                .ToList();
            _context.Subtasks.RemoveRange(subtasksToRemove);
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

            // Remove related TaskTags
            _context.TaskTags.RemoveRange(taskItem.TaskTags);

            // Remove related Subtasks
            _context.Subtasks.RemoveRange(taskItem.Subtasks);

            // Remove the task itself
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
