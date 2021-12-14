using System.Collections.Generic;

namespace DrSproc.Main.Shared
{
    internal class StoredProcInput
    {
        public StoredProc StoredProc { get; set; }

        public IDictionary<string, object> Parameters { get; set; }

        public int? TimeOutSeconds { get; set; }
    }
}