using System;
using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;
using GBG.GameToolkit.AI.StateMachine;
using GBG.GameToolkit.Unity.AI.Parameter;
using UnityEngine;

namespace GBG.GameToolkit.Unity.AI.StateMachine
{
    /// <summary>
    /// 状态机数据。
    /// </summary>
    [Serializable]
    public class UnityStateMachineData : IStateMachineData
    {
        /// <inheritdoc />
        string IStateMachineData.Comment => Comment;
        /// <inheritdoc />
        IBlackboardData IStateMachineData.Blackboard => Blackboard;
        /// <inheritdoc />
        int IStateMachineData.MaxTransitionCountPerEvaluation => MaxTransitionCountPerEvaluation;
        /// <inheritdoc />
        string IStateMachineData.EntryNodeGuid => EntryNodeGuid;
        /// <inheritdoc />
        IReadOnlyList<ISMNodeData> IStateMachineData.Nodes => Nodes;

        /// <summary>
        /// 描述信息。
        /// </summary>
        [TextArea(1, 3)]
        public string Comment;
        /// <summary>
        /// 黑板。
        /// </summary>
        public UnityBlackboardData Blackboard;
        /// <summary>
        /// 每次评估状态机时，可执行的状态转换次数上限。
        /// </summary>
        public int MaxTransitionCountPerEvaluation = 1;
        /// <summary>
        /// 入口节点GUID。
        /// </summary>
        public string EntryNodeGuid;
        /// <summary>
        /// 状态节点列表。
        /// </summary> 
        [SerializeReference]
        public List<ISMNodeData> Nodes = new List<ISMNodeData>();
    }
}