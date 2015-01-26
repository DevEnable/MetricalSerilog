using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace SerilogMetrics
{
    public class DbCommandWrapper : DbCommand
    {
        private static readonly Regex _commandTypeRegex = new Regex(@"^(?:\s*)\w+", RegexOptions.Compiled);
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
            using (LogCommandDetails())
            {
                return _inner.ExecuteReader(behavior);
            }
        }

        public override int ExecuteNonQuery()
        {
            using (LogCommandDetails())
            {
                return _inner.ExecuteNonQuery();
            }
        }

        public override object ExecuteScalar()
        {
            using (LogCommandDetails())
            {
                return _inner.ExecuteScalar();
            }
        }

        private IDisposable LogCommandDetails()
        {
            // It would be better to get this through IoC (or a similar mechanism) with lifecycle per request rather than recreating all of the time.
            var logger = LoggerFactory.GetLoggerConfiguration().CreateLogger();

            var entry = GetLoggedVersionOfCommand();
            logger.Information("Executing {@Command}", entry);

            return logger.BeginTimedOperation("Database Execution", null, LogEventLevel.Information, 
                TimeSpan.FromMilliseconds(10));
        }

        private LoggedCommand GetLoggedVersionOfCommand()
        {
            return new LoggedCommand
            {
                CommandText = this.CommandText,
                CommandType = this.GetCommandType(),
                LoggedParameters = GetLoggedParameters()
            };

        }

        private IEnumerable<LoggedParameter> GetLoggedParameters()
        {
            List<LoggedParameter> parameters = new List<LoggedParameter>(_inner.Parameters.Count);
            parameters.AddRange(from DbParameter parameter in _inner.Parameters
                select new LoggedParameter
                {
                    Name = parameter.ParameterName, Value = parameter.Value?.ToString()
                });

            return parameters;
        }


        private LoggedCommandType GetCommandType()
        {
            // Highly simplified version.
            var match = _commandTypeRegex.Match(this.CommandText);

            if (match.Success)
            {
                switch (match.Value.Trim().ToLower())
                {
                    case "insert":
                        return LoggedCommandType.Insert;
                    case "delete":
                        return LoggedCommandType.Delete;
                    case "select":
                        return LoggedCommandType.Select;
                    case "update":
                        return LoggedCommandType.Update;
                }
            }

            return LoggedCommandType.Unknown;
        }

        private struct LoggedCaller
        {
            public string Type { get; set; }
            public string Method { get; set; }
        }
    }
}
