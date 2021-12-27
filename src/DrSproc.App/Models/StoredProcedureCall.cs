using System.Collections.Generic;

namespace DrSproc.Models
{
    public class StoredProcedureCall
    {
        public string DatabaseName { get; internal set; }

        public string StoredProcedureName { get; internal set; }

        public IDictionary<string, object> Parameters { get; internal set; }

        public int RowsAffected { get; internal set; }
    }
}