using System.Text.Json.Serialization;

namespace TodoBack.Models
{
    public class Subtask
    {
        public int SubtaskId { get; set; }
        public string SubtaskName { get; set; } = "";
        public int TaskItemId { get; set; }
        [JsonIgnore]
        public TaskItem TaskItem { get; set; } = null!;
        public int SubtaskStateId { get; set; }
        [JsonIgnore]
        public SubtaskState SubtaskState { get; set; } = null!;

    }
}
