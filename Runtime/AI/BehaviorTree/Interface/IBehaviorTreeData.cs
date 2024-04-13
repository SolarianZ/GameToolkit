using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树数据。
    /// </summary>
    public interface IBehaviorTreeData
    {
        /// <summary>
        /// 描述信息。
        /// </summary>
        string Comment { get; }
        /// <summary>
        /// 黑板数据。
        /// </summary>
        IBlackboardData Blackboard { get; }
        /// <summary>
        /// 若为true，行为树在运行时使用“主动执行模式”，每帧从根节点开始执行；<br/>
        /// 否则行为树在运行时使用“被动执行模式”，当前节点执行完毕后再触发执行下一个节点。
        /// </summary>
        /// <seealso cref="BTContext.ExecuteInPassiveMode"/>
        /// <seealso cref="BTContext.PassiveExecutionNode"/>
        /// <seealso cref="BTNodeInstanceBase.GetNextPassiveExecutionNode"/>
        bool AlwaysExecuteFromRoot { get; }
        /// <summary>
        /// 根节点GUID。
        /// </summary>
        string RootNodeGuid { get; }
        /// <summary>
        /// 节点列表。
        /// </summary>
        IReadOnlyList<IBTNodeData> Nodes { get; }
    }
}