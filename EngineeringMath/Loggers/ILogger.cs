using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Loggers
{
    public interface ILogger
    {
        void Debug(string title, string message);
        void Error(string title, string message);
        void Warning(string title, string message);
        void Information(string title, string message);
    }
}
