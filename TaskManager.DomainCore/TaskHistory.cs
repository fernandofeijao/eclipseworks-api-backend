namespace TaskManager.DomainCore;

public class TaskHistory
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string? User { get; set; }
    public DateTime ChangeDate { get; set; }
    public string? BeforeChange { get; set; }
    public string? AfterChange { get; set; }
}