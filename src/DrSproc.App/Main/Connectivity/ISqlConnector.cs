using System.Collections.Generic;

namespace DrSproc.Main.Connectivity
{
    internal interface ISqlConnector
    {
        IConnectedSproc CreateConnectedSproc(string connectionString, string procedureName, Dictionary<string, object> parameters, int? commandTimeout = null);
    }
}