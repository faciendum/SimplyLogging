using SimplyLogging.Core.Models;

namespace SimplyLogging.Core.Interfaces
{
    public interface ILoggingStyle
    {
        string ApplyStyle(LogMessage logMessage);
    }
}
