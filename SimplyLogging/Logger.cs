using SimplyLogging.Core;
using SimplyLogging.Core.Enums;
using SimplyLogging.Core.Implementations;
using SimplyLogging.Core.Interfaces;
using SimplyLogging.Core.Models;
using SimplyLogging.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SimplyLogging
{
    public static class Logger
    {
        #region Properties
        private static LogManager LogManager { get; }
        private static DebugLogger DebugLogger { get; }
        private static bool IsDebugMode { get; set; } = true;
        private static bool IsActive { get; set; } = true;
        private static LogLevelKind DefaultLogLevel { get; set; } = LogLevelKind.Trace;

        private static readonly object Sync = new object();

        public static IEnumerable<LogMessage> Messages
        {
            get { return LogManager.Messages; }
        }

        public static ILoggingObjectManager LoggingHandlerManager
        {
            get { return LogManager; }
        }
        #endregion

        static Logger()
        {
            lock (Sync)
            {
                LogManager = new LogManager();
            }
        }

        public static void DefaultInitialization()
        {
            LogManager.AddLoggingObject(new ConsoleLogging()).AddLoggingObject(new FileLogging());
            Log(LogLevelKind.Info, "Default initialization");
        }

        public static void Log(string message = "- no message -")
        {
            Log(DefaultLogLevel, message);
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
            string message = $"Exception -> Message: {exception.Message}\nStackTrace: {exception.StackTrace}";
            Log<TClass>(LogLevelKind.Error, message);
        }

        public static void Log<TClass>(string message) where TClass : class
        {
            Log<TClass>(DefaultLogLevel, message);
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
            LogManager.Write(logMessage);
        }
    }
}
