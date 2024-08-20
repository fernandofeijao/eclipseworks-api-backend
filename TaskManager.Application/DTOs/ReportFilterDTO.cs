namespace TaskManager.Application
{
    public class ReportFilterDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string? User { get; set; }
        public string? ActionUser { get; set; }
    }
}