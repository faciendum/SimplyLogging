using System;

namespace SimplyLogging.Core.Interfaces
{
    public interface ILoggingHandlerManager
    {
        ILoggingHandlerManager AddHandler(ILoggingObject loggingHandler);
        ILoggingHandlerManager AddHandler(ILoggingObject loggingHandler, Predicate<LogMessage> filter);
        bool RemoveHandler(ILoggingObject loggingHandler);
    }
}
