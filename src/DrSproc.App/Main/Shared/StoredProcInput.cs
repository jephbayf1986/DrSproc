using System.Collections.Generic;

namespace DrSproc.Main.Shared
{
    internal class StoredProcInput
    {
        public StoredProc StoredProc { get; private set; }

        public IDictionary<string, object> Parameters { get; private set; }

        public int? TimeOutSeconds { get; private set; }

        public StoredProcInput(StoredProc storedProc, IDictionary<string, object> parameters = null, int? timeoutSeconds = null)
        {
            StoredProc = storedProc;
            Parameters = parameters;
            TimeOutSeconds = timeoutSeconds;
        }
    }
}