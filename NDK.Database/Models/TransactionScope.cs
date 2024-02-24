using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.Models
{
    public class TransactionScope : IDisposable
    {

        private DbConnection? _connection;
        private DbTransaction? _transaction;
        private readonly TransactionScopeOptions? _scopeOptions;
        bool disposed;

        public TransactionScope(DbConnection connection, TransactionScopeOptions? options = null)
        {
            _connection = connection;
            _scopeOptions = options;
            if (options is null)
            {
                _transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            else
            {
                _transaction = connection.BeginTransaction(options.IsolationLevel);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _connection = null;

                    if (_scopeOptions is not null && _scopeOptions.AllowCommit)
                    {
                        _transaction?.Commit();
                    }

                    _transaction?.Dispose();
                    _transaction = null;

                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TransactionScope() 
        { 
            Dispose(false); 
        }
    }
}
