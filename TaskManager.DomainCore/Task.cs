namespace TaskManager.DomainCore;

public class Task
{
    public int Id { get; set; }
    public required string User { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public byte State { get; set; }
    public byte Priority { get; set; }
    public DateTime TargetDate { get; set; }
    public int ProjectId { get; set; }
}