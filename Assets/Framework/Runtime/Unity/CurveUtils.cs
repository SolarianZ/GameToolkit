#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using UnityEngine;

namespace GBG.Framework.Unity
{
    public static class CurveUtils
    {
        public static bool IsNormalized(this AnimationCurve curve, float tolerant = 0.001f)
        {
            if (curve == null)
            {
                return false;
            }

            if (curve.length < 2)
            {
                return false;
            }

            var start = curve[0];
            if (Mathf.Abs(start.time) > tolerant || Mathf.Abs(start.value) > tolerant)
            {
                return false;
            }

            var end = curve[curve.length - 1];
            if (Mathf.Abs(end.time - 1) > tolerant || Mathf.Abs(end.value - 1) > tolerant)
            {
                return false;
            }

            return true;
        }
    }
}
#endif