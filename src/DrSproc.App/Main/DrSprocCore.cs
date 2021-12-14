﻿using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;

namespace DrSproc.Main
{
    internal class DrSprocCore : DrSproc
    {
        private readonly IDbExecutor _dbExecutor;
        private readonly IEntityMapper _entityMapper;

        public DrSprocCore(IDbExecutor executor, IEntityMapper entityMapper)
        {
            _dbExecutor = executor;
            _entityMapper = entityMapper;
        }

        public ITargetDatabase Use<T>() where T : IDatabase, new()
        {
            return new TargetDatabase<T>(_dbExecutor, _entityMapper);
        }
    }
}