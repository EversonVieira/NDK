using System.Data.Common;

namespace NDK.Database.Interfaces
{
    public interface INdkDbConnectionFactory
    {
        DbConnection GetDbConnection();
    }
}