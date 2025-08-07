using TodoBack.Models;
using TodoBack.Models.DTO;
namespace TodoBack.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllAsync();
        Task<TaskDto?> GetByIdAsync(int id);
        Task<List<TaskDto>?> GetTaskByTagId(int id);
        Task<TaskItem> CreateAsync(TaskCreateDto taskItem);
        Task<TaskItem> UpdateAsync(int id, TaskCreateDto taskItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> CompleteAsync(int id);
        Task<bool> AssignTagsAsync(int taskId, List<int> tagIds);
    }
}
