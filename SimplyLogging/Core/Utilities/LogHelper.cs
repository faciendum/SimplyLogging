using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SimplyLogging.Core.Utilities
{
    public static class LogHelper
    {
        public static MethodBase GetCallingMethodBase(StackFrame stackFrame)
        {
            return stackFrame == null ? MethodBase.GetCurrentMethod() : stackFrame.GetMethod();
        }

        public static StackFrame FindStackFrame()
        {
            StackTrace stackTrace = new StackTrace();
            for (int i = 0; i < stackTrace.GetFrames().Count(); i++)
            {
                MethodBase methodBase = stackTrace.GetFrame(i).GetMethod();
                if (methodBase != null)
                {
                    string name = MethodBase.GetCurrentMethod().Name;

                    if (!methodBase.Name.Equals("Log") && !methodBase.Name.Equals(name))
                    {
                        return new StackFrame(i, true);
                    }
                }
            }
            return null;
        }
    }
}
