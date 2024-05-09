using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GBG.GameToolkit.Unity
{
    [Serializable]
    public struct BindingKey : IEquatable<BindingKey>
    {
        /// <summary>
        /// Playable track name.
        /// </summary>
        public string StreamName => _streamName;
        /// <summary>
        /// Playable track binding type full name.
        /// This type must be same with the type specified by the TrackBindingTypeAttribute of the playable track.
        /// Do NOT use subclass.
        /// </summary>
        public string OutputTargetTypeFullName => _outputTargetFullName;
#if ODIN_INSPECTOR
        [LabelText("Name")]
#endif
        [Tooltip("Playable track name.")]
        [SerializeField]
        private string _streamName;
#if ODIN_INSPECTOR
        [LabelText("Type")]
#endif
        [Tooltip("Playable track binding type full name. " +
                 "This type must be same with the type specified by " +
                 "the TrackBindingTypeAttribute of the playable track. " +
                 "Do NOT use subclass.")]
        [SerializeField]
        private string _outputTargetFullName;


        public BindingKey(string streamName, string outputTargetTypeFullName)
        {
            _streamName = streamName;
            _outputTargetFullName = outputTargetTypeFullName;
        }

        public BindingKey(string streamName, Type outputTargetType)
            : this(streamName, outputTargetType.FullName)
        {
        }

        public void Deconstruct(out string streamName, out string outputTargetTypeFullName)
        {
            streamName = _streamName;
            outputTargetTypeFullName = _outputTargetFullName;
        }

        public bool IsTypeMatch(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
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

        public bool Equals(BindingKey other)
        {
            return other._streamName == _streamName && other._outputTargetFullName == _outputTargetFullName;
        }

        public override bool Equals(object obj)
        {
            return obj is BindingKey other && Equals(other);
        }

        public static bool operator ==(BindingKey left, BindingKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BindingKey left, BindingKey right)
        {
            return !left.Equals(right);
        }
    }
}