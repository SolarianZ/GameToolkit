using GBG.GameToolkit.AI.Condition;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机状态转换。
    /// </summary>
    public interface ISMTransitionInstance : IConditionGroupInstance
    {
        /// <summary>
        /// 目标节点GUID。
        /// </summary>
        string DestNodeGuid { get; }
    }
}