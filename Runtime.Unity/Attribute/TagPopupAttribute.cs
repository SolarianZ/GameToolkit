using System;
using System.Diagnostics;
using UnityEngine;

namespace GBG.GameToolkit.Unity
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    public class TagPopupAttribute : PropertyAttribute { }
}
