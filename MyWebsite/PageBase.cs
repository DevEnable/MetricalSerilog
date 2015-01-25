using System;
using System.Web.UI;
using Serilog;
using Serilog.Context;
using SerilogMetrics;

namespace MyWebsite
{
    public class PageBase : Page
    {
        private IDisposable _timing;
        private IDisposable _identifierContext;
        protected readonly ILogger Logger = LoggerFactory.GetLoggerConfiguration().CreateLogger();

        protected string OperationIdentifier { get; } = Guid.NewGuid().ToString();

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            _identifierContext = LogContext.PushProperty("Identifier", this.OperationIdentifier);
            _timing = Logger.BeginTimedOperation(this.GetType().Name, OperationIdentifier);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            CleanupLogging();
        }

        protected override void OnError(EventArgs e)
        {
            CleanupLogging();
            base.OnError(e);
        }

        private void CleanupLogging()
        {
            _timing.Dispose();
            _identifierContext.Dispose();
        }

    }
}