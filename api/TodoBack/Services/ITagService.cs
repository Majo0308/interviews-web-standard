using TodoBack.Models;
using TodoBack.Models.DTO;
namespace TodoBack.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(int id);
        Task<Tag> CreateAsync(Tag tag);
        Task<List<Tag>> AssignTagsAsync(List<int> listTags, TaskItem task);
        Task<List<Tag>> SyncTagsAsync(List<int> listTags, TaskItem task);
        Task<Tag> UpdateAsync(Tag tag);
        Task<bool> DeleteAsync(int id);
        Task<List<Tag>> GetTagsByTaskIdAsync(int taskId);

    }
}
