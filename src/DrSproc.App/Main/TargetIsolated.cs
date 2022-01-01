using DrSproc.Builders;
using DrSproc.Builders.Async;
using DrSproc.Main.Builders;
using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Main.Transactions;
using DrSproc.Transactions;
using System.Data.SqlClient;

namespace DrSproc.Main
{
    internal class TargetIsolated<TDatabase> : ITargetIsolated<TDatabase>
        where TDatabase : IDatabase, new() 
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;

        public TargetIsolated(IDbExecutor dbExecutor, IEntityMapper entityMapper)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;
        }

        public ISprocBuilder Execute(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            using (var connection = GetSqlConnection())
            {
                return new SprocBuilder<TDatabase>(_dbExecutor, _entityMapper, connection, sproc);
            }
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            using (var connection = GetSqlConnection())
            {
                return new AsyncSprocBuilder<TDatabase>(_dbExecutor, _entityMapper, connection, sproc);
            }
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            using (var connection = GetSqlConnection())
            {
                return new SprocBuilder<TDatabase>(_dbExecutor, _entityMapper, connection, sproc);
            }
        }

        public IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            using (var connection = GetSqlConnection())
            {
                return new AsyncSprocBuilder<TDatabase>(_dbExecutor, _entityMapper, connection, sproc);
            }
        }

        public ITargetTransaction BeginTransaction(TransactionIsolation? isolationLevel = null)
        {
            var transaction = new Transaction<TDatabase>();

            transaction.BeginTransaction(isolationLevel);

            return new TargetTransaction<TDatabase>(_dbExecutor, _entityMapper, transaction);
        }

        public ITargetTransaction BeginTransaction(out ITransaction<TDatabase> transaction, TransactionIsolation? isolationLevel = null)
        {
            var transactionInstance = new Transaction<TDatabase>();

            transactionInstance.BeginTransaction(isolationLevel);

            transaction = transactionInstance;

            return new TargetTransaction<TDatabase>(_dbExecutor, _entityMapper, transaction);
        }

        private SqlConnection GetSqlConnection()
        {
            var db = new TDatabase();

            var connection = new SqlConnection(db.GetConnectionString());

            connection.ConnectionString = db.GetConnectionString();

            return connection;
        }
    }
}