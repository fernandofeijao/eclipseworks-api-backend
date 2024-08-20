using System.ComponentModel.DataAnnotations;

namespace TaskManager.DomainCore;

public class Project
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Owner { get; set; }
    public required DateTime CreateDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
}
