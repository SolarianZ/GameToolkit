using System;
using GBG.GameToolkit.AI.Condition;
using GBG.GameToolkit.AI.Parameter;
using GBG.GameToolkit.Unity.AI.Parameter;

namespace GBG.GameToolkit.Unity.AI.Condition
{
    /// <summary>
    /// 黑板参数条件数据。
    /// </summary>
    [Serializable]
    public class ParamConditionData : ConditionDataBase, IParamConditionData
    {
        /// <inheritdoc />
        ParamType IParamConditionData.ParamType => ParamType;
        /// <inheritdoc />
        ConditionOperator IParamConditionData.Operator => Operator;
        /// <inheritdoc />
        string IParamConditionData.LeftParamGuid => LeftParamGuid;
        /// <inheritdoc />
        IParamLiteral IParamConditionData.LeftParamLiteral => LeftParamLiteral;
        /// <inheritdoc />
        string IParamConditionData.RightParamGuid => RightParamGuid;
        /// <inheritdoc />
        IParamLiteral IParamConditionData.RightParamLiteral => RightParamLiteral;

        /// <summary>
        /// 参数类型。
        /// </summary>
        public ParamType ParamType;
        /// <summary>
        /// 参数运算符。<br/>
        /// 运算符必须适用于选定的参数类型。
        /// </summary>
        /// <seealso cref="ConditionHelper.GetValidOperators(ParamType)"/>
        /// <seealso cref="ConditionHelper.CheckOperator(ParamType, ConditionOperator)"/>
        public ConditionOperator Operator;
        /// <summary>
        /// 左运算数参数GUID。<br/>
        /// 目标参数的类型必须与<see cref="ParamType"/>相同。
        /// </summary>
        public string LeftParamGuid;
        /// <summary>
        /// 左运算数参数常量值。<br/>
        /// 目标参数的类型必须与<see cref="ParamType"/>相同。<br/>
        /// 仅在<see cref="LeftParamGuid"/>为空时有效。
        /// </summary>
        public UnityParamLiteral LeftParamLiteral;
        /// <summary>
        /// 右运算数参数GUID。<br/>
        /// 目标参数的类型必须与<see cref="ParamType"/>相同。<br/>
        /// </summary>
        public string RightParamGuid;
        /// <summary>
        /// 右运算数参数常量值。<br/>
        /// 目标参数的类型必须与<see cref="ParamType"/>相同。<br/>
        /// 仅在<see cref="RightParamGuid"/>为空时有效。
        /// </summary>
        public UnityParamLiteral RightParamLiteral;


        /// <inheritdoc/>
        public override IConditionInstance CreateConditionInstance(IBlackboardInstance blackboard)
        {
            ConditionInstanceBase instance = new ParamConditionInstance(blackboard, this);
            return instance;
        }
    }
}