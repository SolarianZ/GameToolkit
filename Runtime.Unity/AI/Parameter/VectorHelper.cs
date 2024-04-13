using GBG.GameToolkit.AI.Parameter;
using UVector4 = UnityEngine.Vector4;
using UQuaternion = UnityEngine.Quaternion;

namespace GBG.GameToolkit.AI.Unity.Parameter
{
    public static class VectorHelper
    {
        public static Vector32 ToVector32(this UVector4 v)
        {
            return new Vector32(v.x, v.y, v.z, v.w);
        }

        public static UVector4 ToVector4(this Vector32 v)
        {
            return new UVector4(v.X, v.Y, v.Z, v.W);
        }

        public static Vector32 ToVector32(this UQuaternion v)
        {
            return new Vector32(v.x, v.y, v.z, v.w);
        }

        public static UQuaternion ToQuaternion(this Vector32 v)
        {
            return new UQuaternion(v.X, v.Y, v.Z, v.W);
        }
    }
}