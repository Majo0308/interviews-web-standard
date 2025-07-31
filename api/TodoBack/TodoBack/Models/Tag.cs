using System.ComponentModel.DataAnnotations;

namespace TodoBack.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required]
        public string TagName { get; set; } = "";
        public string TagColor { get; set; } = "";
        public List<TaskTag> TaskTags { get; set; } = new();

    }
}
