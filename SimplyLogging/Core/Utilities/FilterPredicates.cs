using SimplyLogging.Core.Enums;
using SimplyLogging.Core.Models;
using System;

namespace SimplyLogging.Core.Utilities
{
    internal static class FilterPredicates
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

    internal class FilterByLevel
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
