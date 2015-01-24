using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using Dapper;
using MyWebsite.Models;

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
            return _connection.Query<Shipper>(
                "SELECT ShipperId, CompanyName, Phone FROM Shippers WHERE CompanyName = @name",
                new {name}).SingleOrDefault();
        }
    }
}