namespace TaskManager.Application
{
    public class BaseTaskDiscussionDTO
    {
        public string? Comment { get; set; }
        public string? User { get; set; }
        public int TaskId { get; set; }
    }

    public class TaskDiscussionDTO : BaseTaskDiscussionDTO
    {
        public int Id { get; set; }
    }

    public class NewTaskDiscussionDTO : BaseTaskDiscussionDTO
    {
        public string? ActionUser { get; set; }
    }
}