using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件数据。
    /// </summary>
    public interface IConditionData
    {
        /// <summary>
        /// 创建此条件实例。
        /// </summary>
        /// <param name="blackboard"></param>
        /// <returns></returns>
        IConditionInstance CreateConditionInstance(IBlackboardInstance blackboard);
    }
}