#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
namespace GBG.GameToolkit.Unity
{
    public static class Converters
    {
        public static System.Numerics.Vector2 ToSystem(this UnityEngine.Vector2 value) => new System.Numerics.Vector2(value.x, value.y);
        public static System.Numerics.Vector3 ToSystem(this UnityEngine.Vector3 value) => new System.Numerics.Vector3(value.x, value.y, value.z);
        public static System.Numerics.Vector4 ToSystem(this UnityEngine.Vector4 value) => new System.Numerics.Vector4(value.x, value.y, value.z, value.w);
        public static System.Numerics.Quaternion ToSystem(this UnityEngine.Quaternion value) => new System.Numerics.Quaternion(value.x, value.y, value.z, value.w);

        public static UnityEngine.Vector2 ToUnity(this System.Numerics.Vector2 value) => new UnityEngine.Vector2(value.X, value.Y);
        public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector3 value) => new UnityEngine.Vector3(value.X, value.Y, value.Z);
        public static UnityEngine.Vector4 ToUnity(this System.Numerics.Vector4 value) => new UnityEngine.Vector4(value.X, value.Y, value.Z, value.W);
        public static UnityEngine.Quaternion ToUnity(this System.Numerics.Quaternion value) => new UnityEngine.Quaternion(value.X, value.Y, value.Z, value.W);
    }
}
#endif