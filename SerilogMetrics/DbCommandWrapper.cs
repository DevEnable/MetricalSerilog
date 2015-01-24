using System;
using System.Data;
using System.Data.Common;

namespace SerilogMetrics
{
    public class DbCommandWrapper : DbCommand
    {
        private readonly DbCommand _inner;

        public DbCommandWrapper(DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            _inner = command;
        }

        public override void Prepare()
        {
            _inner.Prepare();
        }

        public override string CommandText
        {
            get { return _inner.CommandText; }
            set { _inner.CommandText = value; }
        }

        public override int CommandTimeout
        {
            get { return _inner.CommandTimeout; }
            set { _inner.CommandTimeout = value; }
        }

        public override CommandType CommandType
        {
            get { return _inner.CommandType; }
            set { _inner.CommandType = value; }
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get { return _inner.UpdatedRowSource; }
            set { _inner.UpdatedRowSource = value; }
        }

        protected override DbConnection DbConnection
        {
            get { return _inner.Connection; }
            set { _inner.Connection = value; }
        }

        protected override DbParameterCollection DbParameterCollection => _inner.Parameters;

        protected override DbTransaction DbTransaction
        {
            get { return _inner.Transaction; }
            set { _inner.Transaction = value; }
        }

        public override bool DesignTimeVisible
        {
            get { return _inner.DesignTimeVisible; }
            set { _inner.DesignTimeVisible = value; }
        }

        public override void Cancel()
        {
            _inner.Cancel();
        }

        protected override DbParameter CreateDbParameter()
        {
            return _inner.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return _inner.ExecuteReader(behavior);
        }

        public override int ExecuteNonQuery()
        {
            return _inner.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            return _inner.ExecuteScalar();
        }
    }
}
