namespace TaskManager.Application
{
    public class BaseProjectDTO
    {
        public string? Name { get; set; }
        public string? Owner { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }

    public class ProjectDTO : BaseProjectDTO
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }
    }

    public class NewProjectDTO : BaseProjectDTO
    {

    }
}