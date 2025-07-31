using TodoBack.Models;
namespace TodoBack.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(int id);
        Task<Tag> CreateAsync(Tag tag);
        Task<Tag> UpdateAsync(Tag tag);
        Task<bool> DeleteAsync(int id);
        Task<List<Tag>> GetTagsByTaskIdAsync(int taskId);

    }
}
