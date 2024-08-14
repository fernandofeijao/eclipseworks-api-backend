namespace TaskManager.DomainCore;

public class Task
{
    public int Id { get; set; }
    public string? User { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public byte State { get; set; }
    public byte Priority { get; set; }
    public DateTime TargetDate { get; set; }
    public int ProjectId { get; set; }
}