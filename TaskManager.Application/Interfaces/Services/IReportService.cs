using FluentResults;

namespace TaskManager.Application
{
    public interface IReportService
    {
        Task<Result<List<TaskReportDTO>>> GetTaskByUser(ReportFilterDTO reportDto);
    }
}
