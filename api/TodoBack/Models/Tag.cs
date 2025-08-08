using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoBack.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required]
        public string TagName { get; set; } = "";
        public string TagColor { get; set; } = "";
        [JsonIgnore]
        public List<TaskTag> TaskTags { get; set; } = new();

    }
}
