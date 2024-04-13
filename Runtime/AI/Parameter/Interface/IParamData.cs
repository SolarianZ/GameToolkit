using System;
using GBG.GameToolkit.AI.Common;

namespace GBG.GameToolkit.AI.Parameter
{
    public interface IParamData : IUniqueItem
    {
        string Name { get; }
        IParamLiteral Literal { get; }

        string ToDefaultString()
        {
            switch (Literal.Type)
            {
                case ParamType.Float32:
                    return $"{Guid} {Name}: {Literal.GetFloat32()}";
                case ParamType.Int32:
                    return $"{Guid} {Name}: {Literal.GetInt32()}";
                case ParamType.Bool:
                    return $"{Guid} {Name}: {Literal.GetBool()}";
                case ParamType.Vector32:
                    return $"{Guid} {Name}: {Literal.GetVector32()}";
                case ParamType.String:
                    return $"{Guid} {Name}: {Literal.GetString()}";
                case ParamType.Object:
                    return $"{Guid} {Name}: {Literal.GetObject()}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), Literal.Type, null);
            }
        }
    }
}