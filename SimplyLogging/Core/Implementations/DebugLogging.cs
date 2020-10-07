using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Models;
using SimplyLogging.Core.Styles;
using System;

namespace SimplyLogging.Core.Implementations
{
    public class DebugLogging : ILoggingObject
    {
        private readonly ILoggingStyle _logStyle;

        public DebugLogging() : this(new DefaultStyle()) { }

        public DebugLogging(ILoggingStyle logStyle)
        {
            _logStyle = logStyle;
        }

        public void Write(LogMessage logMessage)
        {
            System.Diagnostics.Debug.WriteLine(_logStyle.ApplyStyle(logMessage));
        }
    }
}
