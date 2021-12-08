using System;
using System.Collections.Generic;
using System.Text;

namespace DrSproc.Main.Connectivity
{
    internal class SqlConnector : ISqlConnector
    {
        public IConnectedSproc CreateConnectedSproc(string connectionString, string procedureName, Dictionary<string, object> parameters, int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }
    }
}
