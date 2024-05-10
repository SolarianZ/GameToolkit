using UnityEngine;
using UnityEngine.Assertions;
using UDebug = UnityEngine.Debug;
using UObject = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GBG.GameToolkit.Unity
{
    /// <summary>
    /// Default Unity debugger.
    /// If you don't want to use this default debugger, please add scripting define symbol 'GBG_DONT_USE_UNITY_DEBUGGER' to your ProjectSettings.
    /// </summary>
    public static class UnityDebugger
    {
#if UNITY_EDITOR && !GBG_DONT_USE_UNITY_DEBUGGER
        [InitializeOnLoadMethod]
#endif
#if !GBG_DONT_USE_UNITY_DEBUGGER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif
        static void AutoRegister()
        {
            Debugger.AssertionFailureHandler = AssertionFailureHandler;
            Debugger.Logger = LogHandler;
        }

#if UNITY_2022_3_OR_NEWER
        [HideInCallstack] 
#endif
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

#if UNITY_2022_3_OR_NEWER
        [HideInCallstack] 
#endif
        public static void AssertionFailureHandler(string message)
        {
            Assert.IsTrue(false, message);
        }
    }
}