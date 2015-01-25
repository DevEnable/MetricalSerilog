using System.Collections.Generic;

namespace SerilogMetrics
{
    public class LoggedCommand
    {
        public string CommandText { get; set; }

        public LoggedCommandType CommandType { get; set; }

        public IEnumerable<LoggedParameter> LoggedParameters { get; set; } 
    }
}
