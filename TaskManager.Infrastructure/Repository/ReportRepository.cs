using Dapper;
using TaskManager.Application;
using TaskManager.DomainCore;

namespace TaskManager.Infrastructure;

public class ReportRepository : IReportRepository
{
    private readonly DbSession _dbSession;
    public ReportRepository(DbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<List<TaskReportDTO>> GetTaskByUserAsync(ReportFilterDTO reportFilterDto)
    {
        string sql = @$"
			SELECT
                SUM(ALLSTATUS.TasksNew) AS TasksNew,
                SUM(ALLSTATUS.TasksActive) AS TasksActive,
                SUM(ALLSTATUS.TasksClosed) AS TasksClosed,
                ALLSTATUS.[User],
                ALLSTATUS.[Month],
                ALLSTATUS.[Year]
            FROM
            (SELECT 
                COUNT(*) AS TasksNew,
                0 AS TasksActive,
                0 AS TasksClosed, 
                U.[Id] AS [User], 
                MONTH(T.TargetDate) AS [Month],
                YEAR(T.TargetDate) AS [Year]
            FROM TASK T
                INNER JOIN [USER] U
                    ON T.[User] = U.Id
            WHERE {(byte)TaskStateEnum.New} = T.[State] AND T.TargetDate BETWEEN @StartDate AND @FinishDate
            GROUP BY U.[Id], MONTH(T.TargetDate), YEAR(T.TargetDate)
            UNION
            SELECT 
                0 AS TasksNew,
                COUNT(*)  AS TasksActive,
                0 AS TasksClosed,  
                U.[Id] AS [User], 
                MONTH(T.TargetDate) AS [Month],
                YEAR(T.TargetDate) AS [Year]
            FROM TASK T
                INNER JOIN [USER] U
                    ON T.[User] = U.Id
            WHERE {(byte)TaskStateEnum.Active} = T.[State] AND T.TargetDate BETWEEN @StartDate AND @FinishDate
            GROUP BY U.[Id], MONTH(T.TargetDate), YEAR(T.TargetDate)
            UNION
            SELECT 
                0 AS TasksNew,
                0 AS TasksActive,
                COUNT(*) AS TasksClosed,  
                U.[Id] AS [User], 
                MONTH(T.TargetDate) AS [Month],
                YEAR(T.TargetDate) AS [Year]
            FROM TASK T
                INNER JOIN [USER] U
                    ON T.[User] = U.Id
            WHERE {(byte)TaskStateEnum.Closed} = T.[State] AND T.TargetDate BETWEEN @StartDate AND @FinishDate
            GROUP BY U.[Id], MONTH(T.TargetDate), YEAR(T.TargetDate)
            ) AS ALLSTATUS
            GROUP BY
                ALLSTATUS.[User],
                ALLSTATUS.[Month],
                ALLSTATUS.[Year];";

        return (await _dbSession.Connection.QueryAsync<TaskReportDTO>(sql, reportFilterDto, _dbSession.Transaction)).ToList();
    }
}
