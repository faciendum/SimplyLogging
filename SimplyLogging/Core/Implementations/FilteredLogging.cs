using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Models;
using System;

namespace SimplyLogging.Core.Implementations
{
    public class FilteredLogging : ILoggingObject
    {
        public Predicate<LogMessage> Filter { get; set; }
        public ILoggingObject LoggingObject { get; set; }

        public void Write(LogMessage logMessage)
        {
            if (Filter(logMessage))
                LoggingObject.Write(logMessage);
        }
    }
}
