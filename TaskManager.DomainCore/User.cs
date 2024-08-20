namespace TaskManager.DomainCore;

public class User
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public byte Profile { get; set; }
}