using SimplyLogging.Core.Enums;
using SimplyLogging.Core.Styles;
using System;

namespace SimplyLogging.Core.Models
{
    public sealed class LogMessage
    {
        public DateTime DateTime { get; private set; }
        public LogLevelKind LogLevel { get; private set; }
        public string Text { get; private set; }
        public string CallingClass { get; private set; }
        public string CallingMethod { get; private set; }
        public int LineNumber { get; private set; }

        public LogMessage(LogLevelKind logLevel, string text, DateTime dateTime, string callingClass, string callingMethod, int lineNumber)
        {
            LogLevel = logLevel;
            Text = text;
            DateTime = dateTime;
            CallingClass = callingClass;
            CallingMethod = callingMethod;
            LineNumber = lineNumber;
        }

        public override string ToString()
        {
            return new DefaultStyle().ApplyStyle(this);
        }
    }
}
