using TodoBack.Models;
namespace TodoBack.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem taskItem);
        Task<TaskItem> UpdateAsync(TaskItem taskItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> CompleteAsync(int id);
        Task<bool> AssignTagsAsync(int taskId, List<int> tagIds);
    }
}
