using DrSproc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrSproc.Main.Transactions
{
    internal class Transaction : ITransactionActions, ITransaction
    {
        private DateTime? _beingTime;
        private DateTime? _rollbackTime;
        private DateTime? _commitTime;
        private TransactionStatus? _status;

        private ICollection<StoredProcedureCall> _procedureCalls;

        public Transaction()
        {
            _procedureCalls = new List<StoredProcedureCall>();
        }

        public DateTime? BeginTime => _beingTime;

        public DateTime? RollbackTime => _rollbackTime;

        public DateTime? CommitTime => _commitTime;

        public TransactionStatus? Status => _status;

        public int TotalRowsAffected
            => _procedureCalls.Sum(x => x.RowsAffected);

        public IEnumerable<StoredProcedureCall> GetStoredProcedureCallsSoFar()
        {
            return _procedureCalls;
        }

        public void BeginTransaction()
        {
            _beingTime = DateTime.Now;
            _status = TransactionStatus.InProcess;
        }

        public void RollbackTransaction()
        {
            _rollbackTime = DateTime.Now;
            _status |= TransactionStatus.RolledBack;
        }

        public void CommitTransaction()
        {
            _commitTime = DateTime.Now;
            _status |= TransactionStatus.Committed;
        }
    }
}