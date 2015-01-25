using Serilog;

namespace SerilogMetrics
{
    public static class LoggerFactory
    {
        public static LoggerConfiguration GetLoggerConfiguration()
        {
            return new LoggerConfiguration()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .Enrich.FromLogContext()
                .WriteTo.Seq("http://localhost:5341");
        }

    }
}
