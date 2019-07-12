using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace EngineeringMath.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string title, string message)
        {
            Log(title, message, (x) => System.Diagnostics.Debug.WriteLine(x));
        }

        public void Error(string title, string message)
        {
            Log(title, message, (x) => Trace.TraceError(x));
        }

        public void Information(string title, string message)
        {
            Log(title, message, (x) => Trace.TraceInformation(x));
        }

        public void Warning(string title, string message)
        {
            Log(title, message, (x) => Trace.TraceWarning(x));
        }

        private void Log(string title, string message, Action<string> logAction)
        {
            logAction($"[{title}] : {message}");
        }
    }
}
