using TaskManager.DomainCore;

namespace TaskManager.Application
{
    public interface IReportRepository
    {
        Task<List<TaskReportDTO>> GetTaskByUserAsync(ReportFilterDTO reportFilterDto);
    }
}