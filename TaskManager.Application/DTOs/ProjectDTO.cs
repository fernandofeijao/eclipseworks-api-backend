namespace TaskManager.Application
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Owner { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}