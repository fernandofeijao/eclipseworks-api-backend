namespace TaskManager.Application
{
    public class TaskReportDTO
    {
        public string? User { get; set; }
        public int TasksClosed { get; set; }
        public int TasksNew { get; set; }
        public int TasksActive { get; set; }
    }
}