using TaskManager.Application;

namespace TaskManager.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DbSession _session;

        public UnitOfWork(DbSession session)
        {
            _session = session;
        }

        public void BeginTransaction()
        {
            if (!IsTransactionActive())
                _session.Transaction = _session.Connection.BeginTransaction();
        }

        public void Commit()
        {
            _session.Transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _session.Transaction?.Rollback();
            Dispose();
        }

        public void Dispose() => _session.Transaction?.Dispose();

        private bool IsTransactionActive()
        {
            return _session.Transaction?.Connection != null;
        }
    }
}
