using System;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件帮助类。
    /// </summary>
    public static class ConditionHelper
    {
        /// <summary>
        /// 检查给定的运算符是否适用于给定的参数类型。
        /// </summary>
        /// <param name="paramType"></param>
        /// <param name="operator"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool CheckOperator(ParamType paramType, ConditionOperator @operator)
        {
            switch (paramType)
            {
                case ParamType.Float32:
                case ParamType.Int32:
                    return true;
                case ParamType.Bool:
                case ParamType.Vector32:
                case ParamType.String:
                case ParamType.Object:
                    return @operator == ConditionOperator.Equal || @operator == ConditionOperator.NotEqual;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramType), paramType, null);
            }
        }

        /// <summary>
        /// 获取给定的参数类型支持的运算符。
        /// </summary>
        /// <param name="paramType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ConditionOperator[] GetValidOperators(ParamType paramType)
        {
            switch (paramType)
            {
                case ParamType.Float32:
                case ParamType.Int32:
                    return new[]
                    {
                        ConditionOperator.Equal, ConditionOperator.NotEqual,
                        ConditionOperator.GreaterThan, ConditionOperator.LessThan
                    };
                case ParamType.Vector32:
                case ParamType.Bool:
                case ParamType.String:
                case ParamType.Object:
                    return new[]
                    {
                        ConditionOperator.Equal, ConditionOperator.NotEqual
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramType), paramType, null);
            }
        }


        #region Comparation

        public static bool ParamEquals(ParamType paramType, IParamValueProvider lhs, IParamValueProvider rhs)
        {
            float epsilon = IParamInstance.Epsilon;
            switch (paramType)
            {
                case ParamType.Float32:
                    return Math.Abs(lhs.GetFloat32() - rhs.GetFloat32()) < epsilon;
                case ParamType.Int32:
                    return lhs.GetInt32() == rhs.GetInt32();
                case ParamType.Bool:
                    return lhs.GetBool() == rhs.GetBool();
                case ParamType.Vector32:
                    Vector32 lhsVector32 = lhs.GetVector32();
                    Vector32 rhsVector32 = rhs.GetVector32();
                    return Math.Abs(lhsVector32.X - rhsVector32.X) < epsilon &&
                        Math.Abs(lhsVector32.Y - rhsVector32.Y) < epsilon &&
                        Math.Abs(lhsVector32.Z - rhsVector32.Z) < epsilon &&
                        Math.Abs(lhsVector32.W - rhsVector32.W) < epsilon;
                case ParamType.String:
                case ParamType.Object:
                    return lhs.GetObject() == rhs.GetObject();
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramType), paramType, null);
            }
        }

        public static bool ParamNotEquals(ParamType paramType, IParamValueProvider lhs, IParamValueProvider rhs)
        {
            return !ParamEquals(paramType, lhs, rhs);
        }

        public static bool ParamGreaterThan(ParamType paramType, IParamValueProvider lhs, IParamValueProvider rhs)
        {
            switch (paramType)
            {
                case ParamType.Float32:
                    return lhs.GetFloat32() > rhs.GetFloat32();
                case ParamType.Int32:
                    return lhs.GetInt32() > rhs.GetInt32();
                case ParamType.Vector32:
                case ParamType.Bool:
                case ParamType.String:
                case ParamType.Object:
                    throw new InvalidOperationException($"Cannot compare param of type '{paramType}' with operator '{ConditionOperator.GreaterThan}'.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramType), paramType, null);
            }
        }

        public static bool ParamLessThan(ParamType paramType, IParamValueProvider lhs, IParamValueProvider rhs)
        {
            switch (paramType)
            {
                case ParamType.Float32:
                    return lhs.GetFloat32() < rhs.GetFloat32();
                case ParamType.Int32:
                    return lhs.GetInt32() < rhs.GetInt32();
                case ParamType.Vector32:
                case ParamType.Bool:
                case ParamType.String:
                case ParamType.Object:
                    throw new InvalidOperationException($"Cannot compare param of type '{paramType}' with operator '{ConditionOperator.LessThan}'.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramType), paramType, null);
            }
        }

        #endregion
    }
}