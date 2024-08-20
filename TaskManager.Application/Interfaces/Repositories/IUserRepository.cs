using TaskManager.DomainCore;

namespace TaskManager.Application
{
    public interface IUserRepository
    {
        Task<User?> GetAsync(string id);
    }
}