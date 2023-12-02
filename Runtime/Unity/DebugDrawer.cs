#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using System.Diagnostics;
using UnityEngine;
using UDebug = UnityEngine.Debug;

namespace GBG.GameToolkit.Unity
{
    public static class DebugDrawer
    {
        public static byte CircleResolution
        {
            get => _circleResolution;
            set
            {
                const byte MIN_RESOLUTION = 12;
                if (value < MIN_RESOLUTION)
                {
                    value = MIN_RESOLUTION;
                    UDebug.LogError($"{nameof(CircleResolution)} must be greater than {MIN_RESOLUTION}.");
                }
                _circleResolution = value;
            }
        }
        public static float DotLineSegmentLength
        {
            get => _dotLineSegmentLength;
            set
            {
                if (value < DotLineGapLength)
                {
                    value = DotLineGapLength;
                    UDebug.LogError($"{nameof(DotLineSegmentLength)} must be greater than {nameof(DotLineGapLength)}.");
                }
                _dotLineSegmentLength = value;
            }
        }
        public static float DotLineGapLength
        {
            get => _dotLineGapLength;
            set
            {
                const float MIN_DOT_LINE_SEG_LEN = 0.01f;
                if (value < 0.01f || value > DotLineSegmentLength)
                {
                    value = Mathf.Clamp(value, MIN_DOT_LINE_SEG_LEN, DotLineSegmentLength);
                    UDebug.LogError($"{nameof(DotLineGapLength)} must be greater than {MIN_DOT_LINE_SEG_LEN} and less than {nameof(DotLineSegmentLength)}.");
                }
                _dotLineGapLength = value;
            }
        }

        private static byte _circleResolution = 32;
        private static float _dotLineSegmentLength = 0.05f;
        private static float _dotLineGapLength = 0.03f;


        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, Color? color = null,
            float duration = 0.0f, bool depthTest = true, bool dotLine = false)
        {
            var c = color ?? Color.white;
            if (!dotLine)
            {
                UDebug.DrawLine(start, end, c, duration, depthTest);
                return;
            }

            var lineLen = (end - start).magnitude;
            if (lineLen <= DotLineSegmentLength)
            {
                UDebug.DrawLine(start, end, c, duration, depthTest);
                return;
            }

            var segAndGapLen = DotLineSegmentLength + DotLineGapLength;
            var wholeSegAndGapCount = (int)(lineLen / segAndGapLen);
            var lastSegLen = lineLen - segAndGapLen * wholeSegAndGapCount;
            if (lastSegLen < DotLineGapLength)
                lastSegLen -= DotLineGapLength;
            else if (lastSegLen > DotLineSegmentLength)
                lastSegLen -= segAndGapLen;
            var firstSegLen = (DotLineSegmentLength + lastSegLen) / 2;
            var dir = (end - start) / lineLen;
            end = start + dir * firstSegLen;
            UDebug.DrawLine(start, end, c, duration, depthTest);
            start = end;
            var currentLen = firstSegLen;
            var isGap = true;
            while (true)
            {
                if (isGap)
                {
                    var gapLen = Mathf.Min(DotLineGapLength, lineLen - currentLen);
                    currentLen += gapLen;
                    start += dir * gapLen;
                }
                else
                {
                    var segLen = Mathf.Min(DotLineSegmentLength, lineLen - currentLen);
                    currentLen += segLen;
                    end = start + dir * segLen;
                    UDebug.DrawLine(start, end, c, duration, depthTest);
                    start = end;
                }

                isGap = !isGap;

                if (currentLen >= lineLen)
                    break;
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawWireBox(Vector3 center, Vector3 extents, Quaternion rotation,
            Color? color = null, float duration = 0.0f, bool depthTest = true, bool dotLine = false)
        {
            var c = color ?? Color.white;
            var forward = rotation * Vector3.forward;
            var up = rotation * Vector3.up;
            var right = rotation * Vector3.right;
            var halfExtents = extents * 0.5f;
            var p0 = center + forward * halfExtents.z + up * halfExtents.y - right * halfExtents.x;
            var p1 = center + forward * halfExtents.z + up * halfExtents.y + right * halfExtents.x;
            var p2 = center - forward * halfExtents.z + up * halfExtents.y + right * halfExtents.x;
            var p3 = center - forward * halfExtents.z + up * halfExtents.y - right * halfExtents.x;
            var p4 = center + forward * halfExtents.z - up * halfExtents.y - right * halfExtents.x;
            var p5 = center + forward * halfExtents.z - up * halfExtents.y + right * halfExtents.x;
            var p6 = center - forward * halfExtents.z - up * halfExtents.y + right * halfExtents.x;
            var p7 = center - forward * halfExtents.z - up * halfExtents.y - right * halfExtents.x;

            DrawLine(p0, p1, c, duration, depthTest, dotLine);
            DrawLine(p1, p2, c, duration, depthTest, dotLine);
            DrawLine(p2, p3, c, duration, depthTest, dotLine);
            DrawLine(p3, p0, c, duration, depthTest, dotLine);

            DrawLine(p4, p5, c, duration, depthTest, dotLine);
            DrawLine(p5, p6, c, duration, depthTest, dotLine);
            DrawLine(p6, p7, c, duration, depthTest, dotLine);
            DrawLine(p7, p4, c, duration, depthTest, dotLine);

            DrawLine(p0, p4, c, duration, depthTest, dotLine);
            DrawLine(p1, p5, c, duration, depthTest, dotLine);
            DrawLine(p2, p6, c, duration, depthTest, dotLine);
            DrawLine(p3, p7, c, duration, depthTest, dotLine);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawWireArc(Vector3 origin, Vector3 fromDir, Vector3 toDir,
            Vector3 normal, float radius, bool closed = false, Color? color = null,
            float duration = 0.0f, bool depthTest = true, bool dotLine = false)
        {
            fromDir.Normalize();
            var c = color ?? Color.white;
            var arcAngle = Vector3.SignedAngle(fromDir, toDir, normal);
            if (arcAngle < 0)
            {
                arcAngle = 360 + arcAngle;
            }

            var arcResolution = (byte)Mathf.Ceil(CircleResolution / (360f / arcAngle));
            var segAngle = arcAngle / arcResolution;
            var segStart = origin + fromDir * radius;
            for (int i = 1; i <= arcResolution; i++)
            {
                var segEnd = origin + Quaternion.AngleAxis(segAngle * i, normal) * fromDir * radius;
                DrawLine(segStart, segEnd, c, duration, depthTest, dotLine);
                segStart = segEnd;
            }

            if (closed)
            {
                DrawLine(origin, origin + fromDir * radius, c, duration, depthTest, dotLine);
                DrawLine(origin, segStart, c, duration, depthTest, dotLine);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawWireCircle(Vector3 origin, Vector3 normal, float radius,
            Color? color = null, float duration = 0.0f, bool depthTest = true, bool dotLine = false)
        {
            var c = color ?? Color.white;
            var tempAxis = Vector3.Cross(normal, Vector3.up).sqrMagnitude < Mathf.Epsilon
                ? Vector3.forward
                : Vector3.up;
            var right = Vector3.Cross(tempAxis, normal);
            var forward = Vector3.Cross(right, normal).normalized;
            var segAngle = 360f / CircleResolution;
            var startPoint = origin + forward * radius;
            var lastPoint = startPoint;
            for (int i = 1; i < CircleResolution; i++)
            {
                var newPoint = origin + Quaternion.AngleAxis(segAngle * i, normal) * forward * radius;
                DrawLine(lastPoint, newPoint, c, duration, depthTest, dotLine);
                lastPoint = newPoint;
            }

            DrawLine(lastPoint, startPoint, c, duration, depthTest, dotLine);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawWireSphere(Vector3 origin, float radius, Quaternion? rotation = null,
            Color? color = null, float duration = 0.0f, bool depthTest = true, bool dotLine = false)
        {
            var q = rotation ?? Quaternion.identity;
            DrawWireCircle(origin, q * Vector3.up, radius, color, duration, depthTest, dotLine);
            DrawWireCircle(origin, q * Vector3.right, radius, color, duration, depthTest, dotLine);
            DrawWireCircle(origin, q * Vector3.forward, radius, color, duration, depthTest, dotLine);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawWireCylinder(Vector3 point0, Vector3 point1, float radius,
            Color? color = null, float duration = 0.0f, bool depthTest = true, bool dotLine = false)
        {
            var c = color ?? Color.white;
            var dir = point1 - point0;
            var q = Quaternion.FromToRotation(Vector3.forward, dir);
            var p0 = point0 + q * Vector3.up * radius;
            var p1 = point0 + q * Vector3.down * radius;
            var p2 = point0 + q * Vector3.left * radius;
            var p3 = point0 + q * Vector3.right * radius;
            var p4 = point1 + q * Vector3.up * radius;
            var p5 = point1 + q * Vector3.down * radius;
            var p6 = point1 + q * Vector3.left * radius;
            var p7 = point1 + q * Vector3.right * radius;
            DrawLine(p0, p4, c, duration, depthTest, dotLine);
            DrawLine(p1, p5, c, duration, depthTest, dotLine);
            DrawLine(p2, p6, c, duration, depthTest, dotLine);
            DrawLine(p3, p7, c, duration, depthTest, dotLine);

            DrawWireCircle(point0, point0 - point1, radius, c, duration, depthTest, dotLine);
            DrawWireCircle(point1, point1 - point0, radius, c, duration, depthTest, dotLine);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawWireCapsule(Vector3 point0, Vector3 point1, float radius,
            Color? color = null, float duration = 0.0f, bool depthTest = true, bool dotLine = false)
        {
            var c = color ?? Color.white;
            var dir = point1 - point0;
            var q = Quaternion.FromToRotation(Vector3.forward, dir);
            var p0 = point0 + q * Vector3.up * radius;
            var p1 = point0 + q * Vector3.down * radius;
            var p2 = point0 + q * Vector3.left * radius;
            var p3 = point0 + q * Vector3.right * radius;
            var p4 = point1 + q * Vector3.up * radius;
            var p5 = point1 + q * Vector3.down * radius;
            var p6 = point1 + q * Vector3.left * radius;
            var p7 = point1 + q * Vector3.right * radius;
            DrawLine(p0, p4, c, duration, depthTest, dotLine);
            DrawLine(p1, p5, c, duration, depthTest, dotLine);
            DrawLine(p2, p6, c, duration, depthTest, dotLine);
            DrawLine(p3, p7, c, duration, depthTest, dotLine);

            DrawWireCircle(point0, point0 - point1, radius, c, duration, depthTest, dotLine);
            DrawWireCircle(point1, point1 - point0, radius, c, duration, depthTest, dotLine);

            DrawWireArc(point0, p0 - point0, p1 - point0, p2 - p3, radius, false, c, duration, depthTest, dotLine);
            DrawWireArc(point0, p2 - point0, p3 - point0, p1 - p0, radius, false, c, duration, depthTest, dotLine);
            DrawWireArc(point1, p4 - point1, p5 - point1, p7 - p6, radius, false, c, duration, depthTest, dotLine);
            DrawWireArc(point1, p6 - point1, p7 - point1, p4 - p5, radius, false, c, duration, depthTest, dotLine);
        }
    }
}
#endif