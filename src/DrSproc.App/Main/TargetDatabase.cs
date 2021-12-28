using DrSproc.Builders;
using DrSproc.Builders.Async;
using DrSproc.Main.Builders;
using DrSproc.Main.Builders.Async;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using System.Data.SqlClient;

namespace DrSproc.Main
{
    internal class TargetDatabase<TDatabase> : ITargetDatabase
        where TDatabase : IDatabase, new() 
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;

        public TargetDatabase(IDbExecutor dbExecutor, IEntityMapper entityMapper)
        {
            _dbExecutor = dbExecutor;
            _entityMapper = entityMapper;
        }

        public ISprocBuilder Execute(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            using (var connection = GetSqlConnection())
            {
                return new SprocBuilder<TDatabase>(_dbExecutor, _entityMapper, connection, null, sproc);
            }
        }

        public IAsyncSprocBuilder ExecuteAsync(string storedProcedureName)
        {
            var sproc = new StoredProc(storedProcedureName);

            using (var connection = GetSqlConnection())
            {
                return new AsyncSprocBuilder<TDatabase>(_dbExecutor, _entityMapper, connection, null, sproc);
            }
        }

        public ISprocBuilder Execute(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            using (var connection = GetSqlConnection())
            {
                return new SprocBuilder<TDatabase>(_dbExecutor, _entityMapper, connection, null, sproc);
            }
        }

        public IAsyncSprocBuilder ExecuteAsync(string schemaName, string storedProcedureName)
        {
            var sproc = new StoredProc(schemaName, storedProcedureName);

            using (var connection = GetSqlConnection())
            {
                return new AsyncSprocBuilder<TDatabase>(_dbExecutor, _entityMapper, connection, null, sproc);
            }
        }

        public ITargetTransaction BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        public ITargetTransaction BeginTransaction(out ITransaction transaction)
        {
            throw new System.NotImplementedException();
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