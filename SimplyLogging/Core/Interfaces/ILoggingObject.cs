using SimplyLogging.Core.Models;

namespace SimplyLogging.Core.Interfaces
{
    public interface ILoggingObject
    {
        void Write(LogMessage logMessage);
    }
}
