using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GBG.GameToolkit.Unity
{
    [Serializable]
    public struct ValidationResult
    {
        public ResultType Type;
        public string Content;
        public object Context;


        public enum ResultType
        {
            // Keep same with UnityEditor.MessageType
            None,
            Info,
            Warning,
            Error
        }
    }

    public interface IValidatable
    {
        void Validate([NotNull] List<ValidationResult> results);
    }
}
