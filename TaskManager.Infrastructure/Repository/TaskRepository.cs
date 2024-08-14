using Dapper;
using TaskManager.Application;
using TaskManager.DomainCore;

namespace TaskManager.Infrastructure;

public class TaskRepository : ITaskRepository
{
    private readonly DbSession _dbSession;
    public TaskRepository(DbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<TaskManager.DomainCore.Task> AddAsync(TaskManager.DomainCore.Task task)
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

        return await _dbSession.Connection.QuerySingleAsync<TaskManager.DomainCore.Task>(sql, task, _dbSession.Transaction);
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

    public async Task<List<DomainCore.Task>> GetAllByProject(int projectId)
    {
        string sql = @"
			SELECT
                Id,
                User,
                Title,
                Description,
                State,
                Priority,
                TargetDate,
                ProjectId
            FROM TASK
            WHERE ProjectId = @projectId;";

        return (await _dbSession.Connection.QueryAsync<DomainCore.Task>(sql, new { projectId }, _dbSession.Transaction)).ToList();
    }

    public async Task<TaskManager.DomainCore.Task?> GetAsync(int id)
    {
        string sql = @"
			SELECT
                Id,
                User,
                Title,
                Description,
                State,
                Priority,
                TargetDate,
                ProjectId
            FROM TASK
            WHERE Id = @id;";

        return await _dbSession.Connection.QuerySingleOrDefaultAsync<TaskManager.DomainCore.Task>(sql, new { id }, _dbSession.Transaction);
    }

    public async Task<List<TaskDiscussion>> GetDiscussionAsync(int taskId)
    {
        string sql = @"
			SELECT
                Id,
                Comment,
                User,
                TaskId
            FROM TASK_DISCUSSION
            WHERE TaskId = @taskId;";

        return (await _dbSession.Connection.QueryAsync<DomainCore.TaskDiscussion>(sql, new { taskId }, _dbSession.Transaction)).ToList();
    }

    public async Task<List<TaskHistory>> GetHistoryAsync(int taskId)
    {
        string sql = @"
			SELECT
                Id,
                TaskId
                User,
                ChangeDate,
                BeforeChange,
                AfterChange
            FROM TASK_HISTORY
            WHERE TaskId = @taskId;";

        return (await _dbSession.Connection.QueryAsync<DomainCore.TaskHistory>(sql, new { taskId }, _dbSession.Transaction)).ToList();
    }

    public async Task<int> RemoveAsync(int id)
    {
        string sql = @"
            DELETE FROM TASK WHERE Id = @id;";

        return await _dbSession.Connection.ExecuteAsync(sql, id, _dbSession.Transaction);
    }

    public Task<DomainCore.Task> UpdateAsync(DomainCore.Task task)
    {
        throw new NotImplementedException();
    }
}
