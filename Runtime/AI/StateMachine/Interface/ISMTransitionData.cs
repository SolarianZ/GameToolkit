using GBG.GameToolkit.AI.Condition;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机状态转换数据。
    /// </summary>
    public interface ISMTransitionData : IConditionGroupData
    {
        /// <summary>
        /// 目标节点GUID。
        /// </summary>
        string DestNodeGuid { get; }
    }
}