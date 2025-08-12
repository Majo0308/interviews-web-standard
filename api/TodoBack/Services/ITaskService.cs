using TodoBack.Models;
using TodoBack.Models.DTO;
namespace TodoBack.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllAsync();
        Task<List<TaskDto>> GetAllTodayAsync();
        Task<TaskDto?> GetByIdAsync(int id);
        Task<List<TaskDto>?> GetTaskByTagId(int id);
        Task<TaskDto> CreateAsync(TaskCreateDto taskItem);
        Task<TaskDto> UpdateAsync(int id, TaskCreateDto taskItem);
        Task<bool> DeleteAsync(int id);
        Task<TaskItem?> CompleteAsync(int id);
        Task<bool> AssignTagsAsync(int taskId, List<int> tagIds);
    }
}
