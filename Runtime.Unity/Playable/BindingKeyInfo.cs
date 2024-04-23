using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GBG.GameToolkit.Unity
{
    [Serializable]
    public struct BindingKeyInfo : IEquatable<BindingKeyInfo>
    {
        /// <summary>
        /// Track name.
        /// </summary>
        public string StreamName => _streamName;
        /// <summary>
        /// Track binding type full name.
        /// </summary>
        public string OutputTargetTypeFullName => _outputTargetFullName;
#if ODIN_INSPECTOR
        [LabelText("Name")]
#endif
        [Tooltip("Track name.")]
        [SerializeField]
        private string _streamName;
#if ODIN_INSPECTOR
        [LabelText("Type")]
#endif
        [Tooltip("Track binding type full name.")]
        [SerializeField]
        private string _outputTargetFullName;
        // Track binding type.
        private Type _type;

        public BindingKeyInfo(string keyName, string outputTargetTypeFullName)
        {
            _streamName = keyName;
            _outputTargetFullName = outputTargetTypeFullName;
            _type = null;
        }

        public BindingKeyInfo(string streamName, Type outputTargetType) : this(streamName, outputTargetType.FullName)
        {
            _type = outputTargetType;
        }

        public bool IsTypeMatch(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (_type != null)
            {
                return _type == type;
            }

            return type.FullName == _outputTargetFullName;
        }

        public override string ToString()
        {
            return $"{_streamName}@{_outputTargetFullName}";
        }

        public override int GetHashCode()
        {
            int hashCode = -423642918;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_streamName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_outputTargetFullName);
            return hashCode;
        }

        public bool Equals(BindingKeyInfo other)
        {
            return other._streamName == _streamName && other._outputTargetFullName == _outputTargetFullName;
        }

        public override bool Equals(object obj)
        {
            return obj is BindingKeyInfo other && Equals(other);
        }

        public static bool operator ==(BindingKeyInfo left, BindingKeyInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BindingKeyInfo left, BindingKeyInfo right)
        {
            return !left.Equals(right);
        }
    }
}