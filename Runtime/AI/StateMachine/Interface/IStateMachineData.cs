using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机数据。
    /// </summary>
    public interface IStateMachineData
    {
        /// <summary>
        /// 描述信息。
        /// </summary>
        string Comment { get; }
        /// <summary>
        /// 黑板。
        /// </summary>
        IBlackboardData Blackboard { get; }
        /// <summary>
        /// 每次评估状态机时，可执行的状态转换次数上限。
        /// </summary>
        int MaxTransitionCountPerEvaluation { get; }
        /// <summary>
        /// 入口节点GUID。
        /// </summary>
        string EntryNodeGuid { get; }
        /// <summary>
        /// 状态节点列表。
        /// </summary>
        IReadOnlyList<ISMNodeData> Nodes { get; }
    }
}