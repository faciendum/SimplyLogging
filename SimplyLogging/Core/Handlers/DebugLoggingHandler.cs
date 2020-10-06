using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Styles;
using System;

namespace SimplyLogging.Core.Handlers
{
    public class DebugLoggingHandler : ILoggingHandler
    {
        private readonly ILoggingStyle _logStyle;

        public DebugLoggingHandler() : this(new DefaultStyle()) { }

        public DebugLoggingHandler(ILoggingStyle logStyle)
        {
            _logStyle = logStyle;
        }

        public void Write(LogMessage logMessage)
        {
            Console.WriteLine(_logStyle.ApplyStyle(logMessage));
        }
    }
}
