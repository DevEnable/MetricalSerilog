using System;
using System.Data;
using System.Data.Common;

namespace SerilogMetrics
{
    public class DbConnectionWrapper : DbConnection
    {
        private readonly DbConnection _innerConnection;

        public DbConnectionWrapper(DbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            _innerConnection = connection;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return _innerConnection.BeginTransaction(isolationLevel);
        }

        public override void Close()
        {
            _innerConnection.Close();
        }

        public override void ChangeDatabase(string databaseName)
        {
            _innerConnection.ChangeDatabase(databaseName);
        }

        public override void Open()
        {
            _innerConnection.Open();
        }

        public override string ConnectionString
        {
            get { return _innerConnection.ConnectionString; }
            set { _innerConnection.ConnectionString = value; }
        }

        public override string Database => _innerConnection.Database;

        public override ConnectionState State => _innerConnection.State;

        public override string DataSource => _innerConnection.DataSource;

        public override string ServerVersion => _innerConnection.ServerVersion;

        protected override DbCommand CreateDbCommand()
        {
            return new DbCommandWrapper(_innerConnection.CreateCommand());
        }
    }
}
