using UnityEngine;
using UnityEngine.Assertions;
using UDebug = UnityEngine.Debug;
using UObject = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GBG.GameToolkit.Unity
{
    public static class UnityDebugger
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void AutoRegister()
        {
            Debugger.AssertionFailureHandler = AssertionFailureHandler;
            Debugger.Logger = LogHandler;
        }

        [HideInCallstack]
        public static void LogHandler(LogLevel level, string tag, string message, object context)
        {
            if (level < Debugger.LogFilterLevel) return;

            var content = Debugger.CombineLogContent(level, tag, message, context);
            switch (level)
            {
                case LogLevel.TRACE:
                case LogLevel.DEBUG:
                case LogLevel.INFO:
                    UDebug.Log(content, context as UObject);
                    break;
                case LogLevel.WARN:
                    UDebug.LogWarning(content, context as UObject);
                    break;
                case LogLevel.ERROR:
                case LogLevel.FATAL:
                    UDebug.LogError(content, context as UObject);
                    break;
                case LogLevel.OFF:
                default:
                    UDebug.LogError($"Invalid log level: {level}. Log content: {content}", context as UObject);
                    break;
            }
        }

        [HideInCallstack]
        public static void AssertionFailureHandler(string message)
        {
            Assert.IsTrue(false, message);
        }
    }
}