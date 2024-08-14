namespace TaskManager.Application
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string? User { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public byte State { get; set; }
        public byte Priority { get; set; }
        public DateTime TargetDate { get; set; }
        public int ProjectId { get; set; }
        public List<TaskDiscussionDTO> Discussion { get; set; } = new List<TaskDiscussionDTO>();
        public List<TaskHistoryDTO> ChangeHistory { get; set; } = new List<TaskHistoryDTO>();
    }
}