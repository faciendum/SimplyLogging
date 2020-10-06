using SimplyLogging.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplyLogging.Core
{
    internal sealed class FilteredHandler : ILoggingHandler
    {
        public Predicate<LogMessage> Filter { get; set; }
        public ILoggingHandler Handler { get; set; }

        public void Write(LogMessage logMessage)
        {
            if (Filter(logMessage))
                Handler.Write(logMessage);
        }
    }

    internal class LogPublisher : ILoggingHandlerManager
    {
        private readonly IList<ILoggingHandler> _loggerHandlers;
        private readonly IList<LogMessage> _messages;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SimpleLogger.Logging.LogPublisher"/> store log messages.
        /// </summary>
        /// <value><c>true</c> if store log messages; otherwise, <c>false</c>. By default is <c>false</c></value>
        public bool StoreLogMessages { get; set; }

        public LogPublisher()
        {
            _loggerHandlers = new List<ILoggingHandler>();
            _messages = new List<LogMessage>();
            StoreLogMessages = false;
        }

        public LogPublisher(bool storeLogMessages)
        {
            _loggerHandlers = new List<ILoggingHandler>();
            _messages = new List<LogMessage>();
            StoreLogMessages = storeLogMessages;
        }

        public void Write(LogMessage logMessage)
        {
            if (StoreLogMessages)
                _messages.Add(logMessage);
            foreach (var loggerHandler in _loggerHandlers)
                loggerHandler.Write(logMessage);
        }

        public ILoggingHandlerManager AddHandler(ILoggingHandler handler)
        {
            if (handler != null)
                _loggerHandlers.Add(handler);
            return this;
        }

        public ILoggingHandlerManager AddHandler(ILoggingHandler handler, Predicate<LogMessage> filter)
        {
            if ((filter == null) || handler == null)
            {
                return this;
            }
              
            return AddHandler(new FilteredHandler()
            {
                Filter = filter,
                Handler = handler
            });
        }

        public bool RemoveHandler(ILoggingHandler handler)
        {
            return _loggerHandlers.Remove(handler);
        }

        public IEnumerable<LogMessage> Messages
        {
            get { return _messages; }
        }
    }
}
