using System;

namespace GBG.GameToolkit.AI.Parameter
{
    [Serializable]
    public class ParamLiteral : IParamLiteral
    {
        ParamType IParamValueProvider.Type => Type;

        public ParamType Type;
        public Vector32 Vector32Value;
        public int Int32Value;
        public object ObjectValue;


        public ParamLiteral(float value)
        {
            Type = ParamType.Float32;
            Vector32Value = new Vector32(value, 0, 0, 0);
        }

        public ParamLiteral(int value)
        {
            Type = ParamType.Int32;
            Int32Value = value;
        }

        public ParamLiteral(bool value)
        {
            Type = ParamType.Bool;
            Int32Value = value ? 1 : 0;
        }

        public ParamLiteral(Vector32 value)
        {
            Type = ParamType.Vector32;
            Vector32Value = value;
        }

        public ParamLiteral(string value)
        {
            Type = ParamType.String;
            ObjectValue = value;
        }

        public ParamLiteral(object value, bool isAssetReference)
        {
            Type = ParamType.Object;
            ObjectValue = value;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case ParamType.Float32:
                    return $"{Type}: {Vector32Value.X}";
                case ParamType.Int32:
                    return $"{Type}: {Int32Value}";
                case ParamType.Bool:
                    return $"{Type}: {Int32Value != 0}";
                case ParamType.Vector32:
                    return $"{Type}: {Vector32Value}";
                case ParamType.String:
                    return $"{Type}: {ObjectValue}";
                case ParamType.Object:
                    return $"{Type}: {ObjectValue}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), Type, null);
            }
        }


        #region Getter

        public float GetFloat32() { return Vector32Value.X; }

        public int GetInt32() { return Int32Value; }

        public bool GetBool() { return Int32Value != 0; }

        public Vector32 GetVector32() { return Vector32Value; }

        public string GetString() { return ObjectValue as string; }

        public object GetObject() { return ObjectValue; }

        public bool TryGetObject<T>(out T value)
        {
            if (ObjectValue is T t)
            {
                value = t;
                return true;
            }

            value = default;
            return false;
        }

        #endregion
    }
}