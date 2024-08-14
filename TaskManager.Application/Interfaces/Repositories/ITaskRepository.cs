using System.Net;
using TaskManager.DomainCore;

namespace TaskManager.Application
{
    public interface ITaskRepository
    {
        Task<List<TaskManager.DomainCore.Task>> GetAllByProject(int projectId);
        Task<TaskManager.DomainCore.Task?> GetAsync(int id);
        Task<TaskManager.DomainCore.Task> AddAsync(TaskManager.DomainCore.Task task);
        Task<TaskManager.DomainCore.Task> UpdateAsync(TaskManager.DomainCore.Task task);
        Task<int> RemoveAsync(int id);
        Task<List<TaskManager.DomainCore.TaskDiscussion>> GetDiscussionAsync(int taskId);
        Task<List<TaskManager.DomainCore.TaskHistory>> GetHistoryAsync(int taskId);
    }
}