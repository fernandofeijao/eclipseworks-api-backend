using FluentResults;

namespace TaskManager.Application
{
    public interface ITaskService
    {
        Task<Result<List<TaskDTO>>> GetAllByProjectAsync(int projectId);
        Task<Result<TaskDTO>> AddAsync(TaskDTO task);
        Task<Result<TaskDTO>> RemoveAsync(int id);
        Task<TaskDTO> UpdateAsync(TaskManager.DomainCore.Task task);
    }
}
