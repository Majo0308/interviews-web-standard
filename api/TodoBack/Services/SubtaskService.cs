using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoBack.Data;
using TodoBack.Models;
using TodoBack.Models.DTO;

namespace TodoBack.Services
{
    public class SubtaskService : ISubtaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SubtaskService(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<List<SubtaskStateDto>> GetAllAsync()
        {
            var subtasks = await _context.Subtasks
                .Include(s => s.TaskItem)              // Incluir tarea relacionada
                .Include(s => s.SubtaskState)      // Incluir estado relacionado (si existe)
                .Select(s => new SubtaskStateDto
                {
                    SubtaskId = s.SubtaskId,
                    SubtaskName = s.SubtaskName,
                    TaskItemId = s.TaskItemId,
                    SubtaskStateId = s.SubtaskStateId,
                    StateName = s.SubtaskState != null ? s.SubtaskState.StateName : null,
                    TaskName = s.TaskItem != null ? s.TaskItem.TaskName : null,
                    TaskDueDate = s.TaskItem != null ? s.TaskItem.TaskDueDate : DateTime.MinValue
                })
                .ToListAsync();

            return subtasks;
        }

        public async Task<SubtaskDto?> GetByIdAsync(int id)
        {
            var subtasks= await _context.Subtasks.FindAsync(id);
            return subtasks == null ? null : _mapper.Map<SubtaskDto>(subtasks);
        }
        public async Task<SubtaskDto> CreateAsync(SubtaskDto subtaskdto)
        {
            var taskExists = await _context.Tasks.AnyAsync(t => t.TaskItemId == subtaskdto.TaskItemId);
            var stateExists = await _context.SubtaskStates.AnyAsync(s => s.SubtaskStateId == subtaskdto.SubtaskStateId);

            if (!taskExists || !stateExists)
                throw new ArgumentException("TaskItem or SubtaskState not found.");

            var subtask = _mapper.Map<Subtask>(subtaskdto);
            _context.Subtasks.Add(subtask);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubtaskDto>(subtask);
        }
        public async Task<SubtaskDto?> UpdateAsync(int id, SubtaskDto subtaskDto)
        {
            var existingSubtask = await _context.Subtasks.FindAsync(id);
            if (existingSubtask == null) return null;

            _mapper.Map(subtaskDto, existingSubtask); 

            await _context.SaveChangesAsync();
            return _mapper.Map<SubtaskDto>(existingSubtask);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var subtask = await _context.Subtasks.FindAsync(id);
            if (subtask == null)
            {
                return false;
            }
            _context.Subtasks.Remove(subtask);
            await _context.SaveChangesAsync();
            return true;


        }
        public async Task<SubtaskDto> ChangeState(SubtaskDto subtaskDto)
        {
            var existingSubtask = await _context.Subtasks.FindAsync(subtaskDto.SubtaskId);
            if (existingSubtask == null)
            {
                throw new KeyNotFoundException("Subtask not found");
            }

            existingSubtask.SubtaskStateId = subtaskDto.SubtaskStateId;
            await _context.SaveChangesAsync();

            return _mapper.Map<SubtaskDto>(existingSubtask);
        }

        public async Task<List<Subtask>> GetByTaskIdAsync(int taskId)
        {
            return await _context.Subtasks
                .Where(s => s.TaskItemId == taskId)
                .ToListAsync();
        }
    }
}
