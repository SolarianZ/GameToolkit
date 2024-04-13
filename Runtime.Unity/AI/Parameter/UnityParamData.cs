using System;
using GBG.GameToolkit.AI.Common;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Unity.Parameter
{
    /// <summary>
    /// 参数数据。
    /// </summary>
    [Serializable]
    public class UnityParamData : IParamData
    {
        /// <inheritdoc />
        string IUniqueItem.Guid => Guid;
        /// <inheritdoc />
        string IParamData.Name => Name;
        /// <inheritdoc />
        IParamLiteral IParamData.Literal => Literal;

        public string Name;
        public string Guid;
        public UnityParamLiteral Literal;


        private UnityParamData(string guid, string name, ParamType type, UnityParamLiteral value)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                throw new ArgumentException("Param guid cannot be null or whitespace.", nameof(guid));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Param name cannot be null or whitespace.", nameof(name));
            }

            Guid = guid;
            Name = name;
            Literal = value;
        }

        public override string ToString()
        {
            return ((IParamData)this).ToDefaultString();
        }


        #region Construction

        public static UnityParamData NewFloat32(float value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Float32_{guid.Substring(0, 8)}";
            return new UnityParamData(guid, name, ParamType.Float32, new UnityParamLiteral(value));
        }

        public static UnityParamData NewInt32(int value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Int32_{guid.Substring(0, 8)}";
            return new UnityParamData(guid, name, ParamType.Int32, new UnityParamLiteral(value));
        }

        public static UnityParamData NewBool(bool value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Bool_{guid.Substring(0, 8)}";
            return new UnityParamData(guid, name, ParamType.Bool, new UnityParamLiteral(value));
        }

        public static UnityParamData NewVector32(Vector32 value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Vector32_{guid.Substring(0, 8)}";
            return new UnityParamData(guid, name, ParamType.Vector32, new UnityParamLiteral(value));
        }

        public static UnityParamData NewString(string value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"String_{guid.Substring(0, 8)}";
            return new UnityParamData(guid, name, ParamType.String, new UnityParamLiteral(value));
        }

        public static UnityParamData NewObject(object value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Object_{guid.Substring(0, 8)}";
            return new UnityParamData(guid, name, ParamType.Object, new UnityParamLiteral(value));
        }

        #endregion
    }
}