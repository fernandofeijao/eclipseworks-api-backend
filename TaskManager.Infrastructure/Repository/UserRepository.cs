using Dapper;
using TaskManager.Application;
using TaskManager.DomainCore;

namespace TaskManager.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly DbSession _dbSession;
    public UserRepository(DbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<TaskManager.DomainCore.User?> GetAsync(string id)
    {
        string sql = @"
			SELECT
                [Id],
                [Name],
                [Profile]
            FROM [USER]
            WHERE [Id] = @id;";

        return await _dbSession.Connection.QuerySingleOrDefaultAsync<TaskManager.DomainCore.User>(sql, new { id }, _dbSession.Transaction);
    }
}
