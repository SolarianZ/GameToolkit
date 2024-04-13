using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 黑板参数条件数据。
    /// </summary>
    public interface IParamConditionData
    {
        /// <summary>
        /// 参数类型。
        /// </summary>
        ParamType ParamType { get; }
        /// <summary>
        /// 参数运算符。<br/>
        /// 运算符必须适用于选定的参数类型。
        /// </summary>
        /// <seealso cref="ConditionHelper.GetValidOperators(ParamType)"/>
        /// <seealso cref="ConditionHelper.CheckOperator(ParamType, ConditionOperator)"/>
        ConditionOperator Operator { get; }
        /// <summary>
        /// 左运算数参数GUID。<br/>
        /// 目标参数的类型必须与<see cref="ParamType"/>相同。
        /// </summary>
        string LeftParamGuid { get; }
        /// <summary>
        /// 左运算数参数常量值。<br/>
        /// 目标参数的类型必须与<see cref="ParamType"/>相同。<br/>
        /// 仅在<see cref="LeftParamGuid"/>为空时有效。
        /// </summary>
        IParamLiteral LeftParamLiteral { get; }
        /// <summary>
        /// 右运算数参数GUID。<br/>
        /// 目标参数的类型必须与<see cref="ParamType"/>相同。<br/>
        /// </summary>
        string RightParamGuid { get; }
        /// <summary>
        /// 右运算数参数常量值。<br/>
        /// 目标参数的类型必须与<see cref="ParamType"/>相同。<br/>
        /// 仅在<see cref="RightParamGuid"/>为空时有效。
        /// </summary>
        IParamLiteral RightParamLiteral { get; }


        /// <summary>
        /// 左运算数是否为字面值（没有绑定到黑板参数）。
        /// </summary>
        /// <returns></returns>
        bool LeftParamIsLiteral()
        {
            return string.IsNullOrEmpty(LeftParamGuid);
        }

        /// <summary>
        /// 右运算数是否为字面值（没有绑定到黑板参数）。
        /// </summary>
        /// <returns></returns>
        bool RightParamIsLiteral()
        {
            return string.IsNullOrEmpty(RightParamGuid);
        }
    }
}