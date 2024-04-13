using System.Diagnostics;

namespace GBG.GameToolkit.AI.Common
{
    public static class Debugger
    {
        public const int LevelTrace = -2;
        public const int LevelDebug = -1;
        public const int LevelInfo = 0;
        public const int LevelWarn = 1;
        public const int LevelError = 2;
        public const int LevelFatal = 3;
        public const int LevelOff = 127;

        public delegate void AssertHandler(bool condition, string message);

        public delegate void LogHandler(int level, string tag, string message, object context);

        public static AssertHandler Asserter { get; set; }
        public static LogHandler Logger { get; set; }

        public static void Log(int level, string tag, string message, object context)
        {
            Logger.Invoke(level, tag, message, context);
        }

        [Conditional("GBG_AI_LOG_TRACE")]
        public static void LogTrace(string tag, string message, object context)
        {
            Log(LevelTrace, tag, message, context);
        }

        [Conditional("GBG_AI_DEBUG")]
        public static void LogDebug(string tag, string message, object context)
        {
            Log(LevelDebug, tag, message, context);
        }

        [Conditional("GBG_AI_DEBUG")]
        public static void Assert(bool condition, string message)
        {
            Asserter?.Invoke(condition, message);
        }
    }
}