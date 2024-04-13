using System;
using GBG.GameToolkit.AI.Parameter;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.AI.Unity.Parameter
{
    [Serializable]
    public class UnityParamLiteral : IParamLiteral
    {
        ParamType IParamValueProvider.Type => Type;

        public ParamType Type;
        public Vector32 Vector32Value;
        public int Int32Value;
        [SerializeReference]
        public object ObjectValue;
        public UObject AssetValue;


        public UnityParamLiteral(float value)
        {
            Type = ParamType.Float32;
            Vector32Value = new Vector32(value, 0, 0, 0);
        }

        public UnityParamLiteral(int value)
        {
            Type = ParamType.Int32;
            Int32Value = value;
        }

        public UnityParamLiteral(bool value)
        {
            Type = ParamType.Bool;
            Int32Value = value ? 1 : 0;
        }

        public UnityParamLiteral(Vector32 value)
        {
            Type = ParamType.Vector32;
            Vector32Value = value;
        }

        public UnityParamLiteral(string value)
        {
            Type = ParamType.String;
            ObjectValue = value;
        }

        public UnityParamLiteral(object value)
        {
            Type = ParamType.Object;
            ObjectValue = value;
            AssetValue = value as UObject;
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