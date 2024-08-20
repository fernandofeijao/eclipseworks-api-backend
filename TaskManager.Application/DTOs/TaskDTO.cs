namespace TaskManager.Application
{
    public class BaseTaskDTO
    {
        public string? User { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public byte Priority { get; set; }
        public DateTime TargetDate { get; set; }
        public int ProjectId { get; set; }
    }

    public class NewTaskDTO : BaseTaskDTO
    {
    }

    public class EditTaskDTO : BaseTaskDTO
    {
        public int Id { get; set; }
        public byte State { get; set; }
        public new byte Priority { get; }
        public string? ActionUser { get; set; }

    }

    public class TaskDTO : BaseTaskDTO
    {
        public int Id { get; set; }
        public byte State { get; set; }
        public List<TaskDiscussionDTO> Discussion { get; set; } = new List<TaskDiscussionDTO>();
        public List<TaskHistoryDTO> ChangeHistory { get; set; } = new List<TaskHistoryDTO>();
    }
}