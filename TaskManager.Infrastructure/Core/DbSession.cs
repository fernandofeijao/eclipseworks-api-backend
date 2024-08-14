using System.Data;
using System.Data.SqlClient;

namespace TaskManager.Infrastructure
{
    public sealed class DbSession : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
