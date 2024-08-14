using FluentResults;

namespace TaskManager.Application
{
    public interface IProjectService
    {
        Task<Result<List<ProjectDTO>>> GetAllAsync(string user);
        Task<Result<ProjectDTO>> AddAsync(ProjectDTO projectDTO);
        Task<Result<ProjectDTO>> RemoveAsync(int id);
    }
}
