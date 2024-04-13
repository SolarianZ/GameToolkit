using System;

namespace GBG.GameToolkit.AI.Parameter
{
    [Serializable]
    public struct Vector32
    {
        public float X;
        public float Y;
        public float Z;
        public float W;


        public Vector32(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {W})";
        }


        #region Operator

        public static implicit operator Vector32(System.Numerics.Vector4 v)
        {
            return new Vector32(v.X, v.Y, v.Z, v.W);
        }

        public static implicit operator System.Numerics.Vector4(Vector32 v)
        {
            return new System.Numerics.Vector4(v.X, v.Y, v.Z, v.W);
        }

        public static implicit operator Vector32(System.Numerics.Quaternion v)
        {
            return new Vector32(v.X, v.Y, v.Z, v.W);
        }

        public static implicit operator System.Numerics.Quaternion(Vector32 v)
        {
            return new System.Numerics.Quaternion(v.X, v.Y, v.Z, v.W);
        }

        #endregion
    }
}