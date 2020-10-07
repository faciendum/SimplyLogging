using SimplyLogging.Core.Implementations;
using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplyLogging.Core
{
    internal class LogManager : ILoggingObjectManager
    {
        private readonly IList<ILoggingObject> _loggingObjects;
        private readonly IList<LogMessage> _messages;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SimpleLogger.Logging.LogPublisher"/> store log messages.
        /// </summary>
        /// <value><c>true</c> if store log messages; otherwise, <c>false</c>. By default is <c>false</c></value>
        public bool StoreLogMessages { get; set; }

        public LogManager()
        {
            _loggingObjects = new List<ILoggingObject>();
            _messages = new List<LogMessage>();
            StoreLogMessages = false;
        }

        public LogManager(bool storeLogMessages)
        {
            _loggingObjects = new List<ILoggingObject>();
            _messages = new List<LogMessage>();
            StoreLogMessages = storeLogMessages;
        }

        public void Write(LogMessage logMessage)
        {
            if (StoreLogMessages)
                _messages.Add(logMessage);
            foreach (var loggerHandler in _loggingObjects)
                loggerHandler.Write(logMessage);
        }

        public ILoggingObjectManager AddLoggingObject(ILoggingObject handler)
        {
            if (handler != null)
                _loggingObjects.Add(handler);
            return this;
        }

        public ILoggingObjectManager AddLoggingObject(ILoggingObject loggingObject, Predicate<LogMessage> filter)
        {
            if ((filter == null) || loggingObject == null)
            {
                return this;
            }

            return AddLoggingObject(new FilteredLogging()
            {
                Filter = filter,
                LoggingObject = loggingObject
            });
        }

        public bool Remove(ILoggingObject loggingObject)
        {
            return _loggingObjects.Remove(loggingObject);
        }

        public IEnumerable<LogMessage> Messages
        {
            get { return _messages; }
        }
    }
}
