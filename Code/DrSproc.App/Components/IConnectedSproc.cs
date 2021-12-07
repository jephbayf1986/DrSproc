using System;
using System.Threading.Tasks;

namespace DrSproc.App.Components
{
    public interface IConnectedSproc
    {
        IConnectedSproc WithTransactionId(Guid transactionId);
        
        IConnectedSproc WithParam(string paramName, object input);

        IConnectedSproc WithParamIfNotNull(string paramName, object input);

        Task Go();

        Task<object> ReturnIdentity(bool allowNull = true);

        Task<T> Return<T>();
    }
}