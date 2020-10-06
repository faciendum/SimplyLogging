using System;

namespace SimplyLogging.Core.Interfaces
{
    public interface ILoggingHandlerManager
    {
        ILoggingHandlerManager AddHandler(ILoggingHandler loggingHandler);
        ILoggingHandlerManager AddHandler(ILoggingHandler loggingHandler, Predicate<LogMessage> filter);
        bool RemoveHandler(ILoggingHandler loggingHandler);
    }
}
