using SimplyLogging.Core.Interfaces;

namespace SimplyLogging.Core.Styles
{
    internal class DefaultStyle : ILoggingStyle
    {
        public string ApplyStyle(LogMessage log)
        {
            return $"{log.DateTime}:yyyy.MM.dd HH:mm:ss.ff| {log.LogLevel}| {log.CallingClass}\t| {log.CallingMethod}\t| {log.LineNumber}\t| {log.Text}";
        }
    }
}
