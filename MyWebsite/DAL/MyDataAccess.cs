using System;
using System.Data.Common;
using System.Linq;
using Dapper;
using MyWebsite.Models;
using Serilog.Context;

namespace MyWebsite.DAL
{
    public class MyDataAccess
    {
        // Should be an IDbConnection, but POC needs to work with DbConnection.
        private readonly DbConnection _connection;

        public MyDataAccess(DbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            
            _connection = connection;
        }

        public Shipper GetShipper(string name)
        {
            // Yet another argument for AOP here...
            // Could pull this information from the downstream LoggedCommand class by using a StackFrame but this is very brittle and who wants to play with a StackFrame anyway?
            using (LogContext.PushProperty("CallingType", "MyDataAccess"))
            {
                using (LogContext.PushProperty("CallingMethod", "GetShipper"))
                {
                    return _connection.Query<Shipper>(
                        "SELECT ShipperId, CompanyName, Phone FROM Shippers WHERE CompanyName = @name",
                        new { name }).SingleOrDefault();
                }
            }
        }
    }
}