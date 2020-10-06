using SimplyLogging.Core;
using SimplyLogging.Core.Enums;
using SimplyLogging.Core.Handlers;
using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SimplyLogging
{
    public static class Logger
    {
        private static LogPublisher LogPublisher {get;}
        private static DebugLogger DebugLogger {get;}
        private static bool IsDebugMode {get; set;} = true;
        private static bool IsActive {get; set;} = true;
        private static LogLevelKind _defaultLevel = LogLevelKind.Trace;
        private static readonly object Sync = new object();

        static Logger()
        {
            lock (Sync)
            {
                LogPublisher = new LogPublisher();
            }
        }

        public static void DefaultInitialization()
        {
            LogPublisher.AddHandler(new ConsoleLoggingHandler()).AddHandler(new FileLoggingHandler());
            Log(LogLevelKind.Info, "Default initialization");
        }

        public static void Log(string message = "- no message -")
        {
            Log(_defaultLevel, message);
        }
      
        public static void Log(LogLevelKind level, string message)
        {
            StackFrame stackFrame = LogHelper.FindStackFrame();
            MethodBase methodBase = LogHelper.GetCallingMethodBase(stackFrame);
            string callingMethod = methodBase.Name;
            string callingClass = methodBase.ReflectedType.Name;
            int lineNumber = stackFrame.GetFileLineNumber();

            Log(level, message, callingClass, callingMethod, lineNumber);
        }

        public static void Log(Exception exception)
        {
            Log(LogLevelKind.Error, exception.Message);
        }

        public static void Log<TClass>(Exception exception) where TClass : class
        {
            string message = $"Log exception -> Message: {exception.Message}\nStackTrace: {exception.StackTrace}";
            Log<TClass>(LogLevelKind.Error, message);
        }

        public static void Log<TClass>(string message) where TClass : class
        {
            Log<TClass>(_defaultLevel, message);
        }

        public static void Log<TClass>(LogLevelKind level, string message) where TClass : class
        {
            StackFrame stackFrame = LogHelper.FindStackFrame();
            MethodBase methodBase = LogHelper.GetCallingMethodBase(stackFrame);
            string callingMethod = methodBase.Name;
            string callingClass = typeof(TClass).Name;
            int lineNumber = stackFrame.GetFileLineNumber();

            Log(level, message, callingClass, callingMethod, lineNumber);
        }

        private static void Log(LogLevelKind level, string message, string callingClass, string callingMethod, int lineNumber)
        {
            if (!IsActive || (!IsDebugMode && level == LogLevelKind.Debug))
                return;

            LogMessage logMessage = new LogMessage(level, message, DateTime.Now, callingClass, callingMethod, lineNumber);
            LogPublisher.Write(logMessage);
        }

        public static IEnumerable<LogMessage> Messages
        {
            get { return LogPublisher.Messages; }
        }

        static class FilterPredicates
        {
            public static bool ByLevelHigher(LogLevelKind logMessLevel, LogLevelKind filterLevel)
            {
                return ((int)logMessLevel >= (int)filterLevel);
            }

            public static bool ByLevelLower(LogLevelKind logMessLevel, LogLevelKind filterLevel)
            {
                return ((int)logMessLevel <= (int)filterLevel);
            }

            public static bool ByLevelExactly(LogLevelKind logMessLevel, LogLevelKind filterLevel)
            {
                return ((int)logMessLevel == (int)filterLevel);
            }

            public static bool ByLevel(LogMessage logMessage, LogLevelKind filterLevel, Func<LogLevelKind, LogLevelKind, bool> filterPred)
            {
                return filterPred(logMessage.LogLevel, filterLevel);
            }
        }

        public class FilterByLevel
        {
            public LogLevelKind FilteredLevel { get; set; }
            public bool ExactlyLevel { get; set; }
            public bool OnlyHigherLevel { get; set; }

            public FilterByLevel(LogLevelKind level)
            {
                FilteredLevel = level;
                ExactlyLevel = true;
                OnlyHigherLevel = true;
            }

            public FilterByLevel()
            {
                ExactlyLevel = false;
                OnlyHigherLevel = true;
            }

            public Predicate<LogMessage> Filter
            {
                get
                {
                    return delegate (LogMessage logMessage)
                    {
                        return FilterPredicates.ByLevel(logMessage, FilteredLevel, delegate (LogLevelKind lm, LogLevelKind fl)
                        {
                            return ExactlyLevel ?
                                FilterPredicates.ByLevelExactly(lm, fl) :
                                (OnlyHigherLevel ?
                                    FilterPredicates.ByLevelHigher(lm, fl) :
                                    FilterPredicates.ByLevelLower(lm, fl)
                                );
                        });
                    };
                }
            }
        }
    }
}
