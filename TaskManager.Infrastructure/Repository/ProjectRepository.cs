using Dapper;
using TaskManager.Application;
using TaskManager.DomainCore;

namespace TaskManager.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    private readonly DbSession _dbSession;
    public ProjectRepository(DbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<Project> AddAsync(Project project)
    {
        string sql = @"
			INSERT INTO PROJECT
			(
                Name,
                Owner,
                CreateDate,
                StartDate,
                FinishDate
			)
			OUTPUT INSERTED.*
			VALUES
			(
                @Name,
                @Owner,
                @CreateDate,
                @StartDate,
                @FinishDate
			);";

        return await _dbSession.Connection.QuerySingleAsync<Project>(sql, project, _dbSession.Transaction);
    }

    public async Task<List<Project>> GetAllAsync(string user)
    {
        string sql = @"
			SELECT
                Id,
                Name,
                Owner,
                CreateDate,
                StartDate,
                FinishDate
            FROM PROJECT
            WHERE Owner = @user;";

        return (await _dbSession.Connection.QueryAsync<Project>(sql, new { user }, _dbSession.Transaction)).ToList();
    }

    public async Task<Project?> GetAsync(int id)
    {
        string sql = @"
			SELECT
                Id,
                Name,
                Owner,
                CreateDate,
                StartDate,
                FinishDate
            FROM PROJECT
            WHERE Id = @id;";

        return await _dbSession.Connection.QuerySingleOrDefaultAsync<Project>(sql, new { id }, _dbSession.Transaction);
    }

    public Task<int> RemoveAsync(int id)
    {
        string sql = @"
            DELETE FROM PROJECT WHERE Id = @id;";

        return _dbSession.Connection.ExecuteAsync(sql, new { id }, _dbSession.Transaction);
    }
}
