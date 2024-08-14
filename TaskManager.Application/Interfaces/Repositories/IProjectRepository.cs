using TaskManager.DomainCore;

namespace TaskManager.Application
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync(string user);
        Task<Project> AddAsync(Project project);
        Task<int> RemoveAsync(int id);
        Task<Project?> GetAsync(int id);
    }
}