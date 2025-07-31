using System.ComponentModel.DataAnnotations;

namespace TodoBack.Models
{
    public class TaskItem
    {
        public int TaskId { get; set; }
        [Required]
        public string TaskName { get; set; } = "";
        public string TaskDescription { get; set; } = "";
        public bool TaskCompleted { get; set; } = false;
        public DateTime TaskCreatedDate { get; set; } = DateTime.Now;
        public DateTime TaskDueDate { get; set; } = DateTime.Now;
        public int TaskPriority { get; set; } = 0;
        public List<TaskTag> TaskTags { get; set; } = new();

    }
}
