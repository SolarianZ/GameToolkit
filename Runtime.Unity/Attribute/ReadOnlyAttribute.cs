using System;
using System.Diagnostics;
using UnityEngine;

namespace GBG.GameToolkit.Unity
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public bool ReadOnlyInEditMode => (_flags & _readOnlyInEditMode) != 0;
        public bool ReadOnlyInPlayMode => (_flags & _readOnlyInPlayMode) != 0;

        private const byte _readOnlyInEditMode = 0b01;
        private const byte _readOnlyInPlayMode = 0b10;
        private readonly byte _flags;


        public ReadOnlyAttribute()
        {
            _flags = _readOnlyInEditMode | _readOnlyInPlayMode;
        }

        public ReadOnlyAttribute(bool readOnlyInEditMode, bool readOnlyInPlayMode)
        {
            _flags = 0;

            if (readOnlyInEditMode)
            {
                _flags |= _readOnlyInEditMode;
            }

            if (readOnlyInPlayMode)
            {
                _flags |= _readOnlyInPlayMode;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyInPlayModeAttribute : ReadOnlyAttribute
    {
        public ReadOnlyInPlayModeAttribute() : base(false, true) { }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyInEditModeAttribute : ReadOnlyAttribute
    {
        public ReadOnlyInEditModeAttribute() : base(true, false) { }
    }
}
