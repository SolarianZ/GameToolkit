using System;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件数据。
    /// </summary>
    [Serializable]
    public abstract class ConditionDataBase : IConditionData
    {
        /// <summary>
        /// 创建此条件实例。
        /// </summary>
        /// <param name="blackboard"></param>
        /// <returns></returns>
        public abstract IConditionInstance CreateConditionInstance(IBlackboardInstance blackboard);

        /// <inheritdoc />
        IConditionInstance IConditionData.CreateConditionInstance(IBlackboardInstance blackboard)
        {
            return CreateConditionInstance(blackboard);
        }
    }
}