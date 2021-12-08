using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DrSproc.Main.Connectivity
{
    internal interface IConnectedSproc
    {
        IEnumerable<T> ExecuteReturnMulti<T>(Func<IDataReader, T> mapper);

        Task<IEnumerable<T>> ExecuteReturnMultiAsync<T>(Func<IDataReader, T> mapper);

        T ExecuteReturnSingle<T>(Func<IDataReader, T> mapper);

        Task<T> ExecuteReturnSingleAsync<T>(Func<IDataReader, T> mapper);

        object ExecuteReturnIdentity();

        Task<object> ExecuteReturnIdentityAsync();

        void Execute();

        Task ExecuteAsync();
    }
}