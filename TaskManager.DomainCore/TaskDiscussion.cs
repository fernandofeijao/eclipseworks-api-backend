namespace TaskManager.DomainCore;

public class TaskDiscussion
{
    public int Id { get; set; }
    public string? Comment { get; set; }
    public string? User { get; set; }
    public int TaskId { get; set; }
}