using System.Data;

namespace DrSproc.Main.Shared
{
    internal class InProcessStoredProc
    {
        public StoredProc StoredProc { get; set; }

        public IDataReader DataReader { get; set; }
    }
}