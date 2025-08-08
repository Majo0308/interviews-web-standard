using TodoBack.Models;
using TodoBack.Models.DTO;

namespace TodoBack.Services
{
    public interface ISubtaskService
    {
        Task<SubtaskDto> CreateAsync(SubtaskDto subtask);
        Task<SubtaskDto?> GetByIdAsync(int id);
        Task<List<SubtaskStateDto>> GetAllAsync();
        Task<SubtaskDto> UpdateAsync(int id, SubtaskDto subtask);
        Task<bool> DeleteAsync(int id);
        Task<SubtaskDto> ChangeState(SubtaskDto subtask);
        
    }
}
