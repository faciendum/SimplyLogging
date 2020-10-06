namespace SimplyLogging.Core.Interfaces
{
    public interface ILoggingHandler
    {
        void Write(LogMessage logMessage);
    }
}
