using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Styles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimplyLogging.Core.Handlers
{
    public class FileLogging : ILoggingObject
    {
        private readonly string _fileName;
        private readonly string _directory;
        private readonly ILoggingStyle _logStyle;

        public FileLogging() : this(CreateFileName()) { }

        public FileLogging(string fileName) : this(fileName, string.Empty) { }

        public FileLogging(string fileName, string directory) : this(new DefaultStyle(), fileName, directory) { }

        public FileLogging(ILoggingStyle logstyle) : this(logstyle, CreateFileName()) { }

        public FileLogging(ILoggingStyle logstyle, string fileName) : this(logstyle, fileName, string.Empty) { }

        public FileLogging(ILoggingStyle logstyle, string fileName, string directory)
        {
            _logStyle = logstyle;
            _fileName = fileName;
            _directory = directory;
        }

        public void Write(LogMessage logMessage)
        {
            if (!string.IsNullOrEmpty(_directory))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(_directory));
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }

            using (StreamWriter writer = new StreamWriter(File.Open(Path.Combine(_directory, _fileName), FileMode.Append)))
            {
                writer.WriteLine(_logStyle.ApplyStyle(logMessage));
            }
        }

        private static string CreateFileName()
        {
            DateTime now = DateTime.Now;
            return $"Log_{now.Year:0000}_{now.Month:00}_{now.Day}-{now.Hour:00}-{now.Minute}_{Guid.NewGuid()}.log";
        }
    }
}
