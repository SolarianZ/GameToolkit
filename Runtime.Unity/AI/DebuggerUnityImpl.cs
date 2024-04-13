using System;
using System.Diagnostics;
using UnityEngine;
using UDebug = UnityEngine.Debug;
using UAssert = UnityEngine.Assertions.Assert;

namespace GBG.GameToolkit.AI.Unity
{
    public static class DebuggerUnityImpl
    {
        [HideInCallstack]
        public static void Assert(bool condition, string message)
        {
            UAssert.IsTrue(condition, message);
        }

        [HideInCallstack]
        public static void Log(int level, string category, string message)
        {
            switch (level)
            {
                case 0:
                    UnityLogDebug(CombineMessage());
                    break;
                case 1:
                    UDebug.Log(CombineMessage());
                    break;
                case 2:
                    UDebug.LogWarning(CombineMessage());
                    break;
                case 3:
                    UDebug.LogError(CombineMessage());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            string CombineMessage()
            {
                return string.IsNullOrEmpty(category) ? message : $"[{category}] {message}";
            }
        }

        [HideInCallstack]
        [Conditional("GBG_AI_DEBUG")]
        private static void UnityLogDebug(string message)
        {
            UDebug.Log(message);
        }
    }
}