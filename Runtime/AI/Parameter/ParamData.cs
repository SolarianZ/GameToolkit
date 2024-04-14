using System;

namespace GBG.GameToolkit.AI.Parameter
{
    /// <summary>
    /// 参数数据。
    /// </summary>
    [Serializable]
    public class ParamData : IParamData
    {
        /// <inheritdoc />
        string IUniqueItem.Guid => Guid;
        /// <inheritdoc />
        string IParamData.Name => Name;
        /// <inheritdoc />
        IParamLiteral IParamData.Literal => Literal;

        public string Name;
        public string Guid;
        public ParamLiteral Literal;


        private ParamData(string guid, string name, ParamType type, ParamLiteral value)
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

        public static ParamData NewFloat32(float value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Float32_{guid.Substring(0, 8)}";
            return new ParamData(guid, name, ParamType.Float32, new ParamLiteral(value));
        }

        public static ParamData NewInt32(int value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Int32_{guid.Substring(0, 8)}";
            return new ParamData(guid, name, ParamType.Int32, new ParamLiteral(value));
        }

        public static ParamData NewBool(bool value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Bool_{guid.Substring(0, 8)}";
            return new ParamData(guid, name, ParamType.Bool, new ParamLiteral(value));
        }

        public static ParamData NewVector32(Vector32 value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Vector32_{guid.Substring(0, 8)}";
            return new ParamData(guid, name, ParamType.Vector32, new ParamLiteral(value));
        }

        public static ParamData NewString(string value, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"String_{guid.Substring(0, 8)}";
            return new ParamData(guid, name, ParamType.String, new ParamLiteral(value));
        }

        public static ParamData NewObject(object value, bool isAssetReference, string name = default)
        {
            string guid = System.Guid.NewGuid().ToString();
            name ??= $"Object_{guid.Substring(0, 8)}";
            return new ParamData(guid, name, ParamType.Object, new ParamLiteral(value, isAssetReference));
        }

        #endregion
    }
}