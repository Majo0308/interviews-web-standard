using System.ComponentModel.DataAnnotations;

namespace TodoBack.Models.DTO
{
    public class TaskDto
    {
            public int TaskItemId { get; set; }
            public string TaskName { get; set; } = "";
            public string TaskDescription { get; set; } = "";
            public bool TaskCompleted { get; set; }
            public DateTime TaskCreatedDate { get; set; }
            public DateTime TaskDueDate { get; set; }
            public int TaskPriority { get; set; }

            public List<Tag> Tags { get; set; } = new();
            public List<SubtaskDto> Subtasks { get; set; } = new();
    }

    public class TaskCreateDto
    {
        public int? TaskItemId { get; set; }
        public string TaskName { get; set; } = "";
        public DateTime TaskDueDate { get; set; }= DateTime.Now;
        public string TaskDescription { get; set; } = "";
        public bool TaskCompleted { get; set; }= false;
        public int TaskPriority { get; set; } = 2;
        public List<int> Tags { get; set; } = new();
        public List<SubtaskDto> Subtasks { get; set; } = new();

    }


}
