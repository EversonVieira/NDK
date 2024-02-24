using System.Data;

namespace NDK.Database.Models
{
    public class TransactionScopeOptions
    {
        public IsolationLevel IsolationLevel { get; set; }
        public bool AllowCommit { get; set; } = true;
    }
}
