using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskOfOptions
{
    internal static class Debug
    {
        private static ManualLogSource _LogSource;

        internal static void Init(ManualLogSource LogSource)
        {
            _LogSource = LogSource;
        }

        internal static void Log(object message, LogLevel logLevel = LogLevel.Info)
        {
            _LogSource.Log(logLevel, $"{message}");
        }
    }
}
