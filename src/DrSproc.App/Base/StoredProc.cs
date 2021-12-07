namespace DrSproc
{
    public abstract class StoredProc
    {
        private string _schemaName;
        private string _sprocName;

        public StoredProc(string sprocName)
        {
            _schemaName = null;
            _sprocName = sprocName;
        }

        public StoredProc(string schemaName, string sprocName)
        {
            _schemaName = schemaName;
            _sprocName = sprocName;
        }

        internal string GetStoredProcFullName()
        {
            if (string.IsNullOrWhiteSpace(_schemaName))
                return _sprocName;

            return $"{_schemaName}.{_sprocName}";
        }
    }
    
    public abstract class StoredProc<T> : StoredProc
    {
        public StoredProc(string sprocName)
            : base (sprocName)
        {
        }

        public StoredProc(string schemaName, string sprocName)
            :base (schemaName, sprocName)
        {
        }
    }
}