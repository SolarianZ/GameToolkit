using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机节点信息。
    /// </summary>
    public class SMNodeInfo
    {
        /// <summary>
        /// 节点名称。
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 节点GUID。
        /// </summary>
        public string Guid { get; }
        /// <summary>
        /// 节点可执行的转换。
        /// </summary>
        internal IReadOnlyList<ISMTransitionInstance> Transitions { get; }


        public SMNodeInfo(ISMNodeData nodeData, IBlackboardInstance blackboard, IReadOnlyList<ISMTransitionInstance> transitions)
        {
            Name = nodeData.Name;
            Guid = nodeData.Guid;
            Transitions = transitions;
        }
    }
}