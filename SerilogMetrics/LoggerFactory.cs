using Serilog;

namespace SerilogMetrics
{
    public class LoggerFactory
    {
        public LoggerConfiguration GetLoggerConfiguration()
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341");
        }

    }
}
