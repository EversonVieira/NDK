using System.Data.Common;

namespace Nedesk.Database.Interfaces
{
    public interface INdkDbConnectionFactory
    {
        DbConnection GetDbConnection();
    }
}