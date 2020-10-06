using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Styles;

namespace SimplyLogging.Core.Handlers
{
   public class ConsoleLogging : ILoggingObject
    {
        private readonly ILoggingStyle _logStyle;

        public ConsoleLogging() : this(new DefaultStyle()) { }

        public ConsoleLogging(ILoggingStyle logStyle)
        {
            _logStyle = logStyle;
        }

        public void Write(LogMessage logMessage)
        {
           System.Diagnostics.Debug.WriteLine(_logStyle.ApplyStyle(logMessage));
        }
    }
}
