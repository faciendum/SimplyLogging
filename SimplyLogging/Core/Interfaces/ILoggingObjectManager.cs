using SimplyLogging.Core.Models;
using System;

namespace SimplyLogging.Core.Interfaces
{
    public interface ILoggingObjectManager
    {
        ILoggingObjectManager AddLoggingObject(ILoggingObject loggingObject);
        ILoggingObjectManager AddLoggingObject(ILoggingObject loggingObject, Predicate<LogMessage> filter);
        bool Remove(ILoggingObject loggingObject);
    }
}
