using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskOfOptions
{
    internal static class Debug
    {
        private static ManualLogSource _logSource;

        internal static void Init(ManualLogSource logSource)
        {
            _logSource = logSource;
        }

        internal static void Log(object message, LogLevel logLevel = LogLevel.Info)
        {
            _logSource.Log(logLevel, $"{message}");
        }
    }
}
