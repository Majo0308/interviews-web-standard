using System.ComponentModel.DataAnnotations;

namespace TodoBack.Models.DTO
{
    public class SubtaskDto
    {
        public int SubtaskId { get; set; } = 0;
        [Required] public string SubtaskName { get; set; } = "";
        [Required] public int TaskItemId { get; set; } = 0;
        public int SubtaskStateId { get; set; } = 1;
        public string? StateName { get; set; } = "";
    }
}
