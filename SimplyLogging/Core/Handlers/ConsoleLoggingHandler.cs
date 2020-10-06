using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Styles;

namespace SimplyLogging.Core.Handlers
{
   public class ConsoleLoggingHandler : ILoggingHandler
    {
        private readonly ILoggingStyle _logStyle;

        public ConsoleLoggingHandler() : this(new DefaultStyle()) { }

        public ConsoleLoggingHandler(ILoggingStyle logStyle)
        {
            _logStyle = logStyle;
        }

        public void Write(LogMessage logMessage)
        {
           System.Diagnostics.Debug.WriteLine(_logStyle.ApplyStyle(logMessage));
        }
    }
}
