using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Configuration;
using SerilogMetrics;

namespace MyWebsite.DAL
{
    /// <summary>
    /// Hack helper code.
    /// </summary>
    public class ConnectionFactory
    {
        public static DbConnection GetConnection()
        {
            return new DbConnectionWrapper(new SqlConnection(WebConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString));
        }

    }
}