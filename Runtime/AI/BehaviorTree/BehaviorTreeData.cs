﻿using System;
using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树数据。
    /// </summary>
    [Serializable]
    public class BehaviorTreeData : IBehaviorTreeData
    {
        /// <inheritdoc />
        string IBehaviorTreeData.Comment => Comment;
        /// <inheritdoc />
        IBlackboardData IBehaviorTreeData.Blackboard => Blackboard;
        /// <inheritdoc />
        bool IBehaviorTreeData.AlwaysExecuteFromRoot => AlwaysExecuteFromRoot;
        /// <inheritdoc />
        string IBehaviorTreeData.RootNodeGuid => RootNodeGuid;
        /// <inheritdoc />
        IReadOnlyList<IBTNodeData> IBehaviorTreeData.Nodes => Nodes;

        /// <summary>
        /// 描述信息。
        /// </summary>
        // #if UNITY_2020_3_OR_NEWER
        //         [UnityEngine.TextArea(1, 3)]
        // #endif
        public string Comment;
        /// <summary>
        /// 黑板数据。
        /// </summary>
        public BlackboardData Blackboard;
        /// <summary>
        /// 若为true，行为树在运行时使用“主动执行模式”，每帧从根节点开始执行；<br/>
        /// 否则行为树在运行时使用“被动执行模式”，当前节点执行完毕后再触发执行下一个节点。
        /// </summary>
        /// <seealso cref="BTContext.ExecuteInPassiveMode"/>
        /// <seealso cref="BTContext.PassiveExecutionNode"/>
        /// <seealso cref="BTNodeInstanceBase.GetNextPassiveExecutionNode"/>
        public bool AlwaysExecuteFromRoot = true;
        /// <summary>
        /// 根节点GUID。
        /// </summary>
        public string RootNodeGuid;
        /// <summary>
        /// 节点列表。
        /// </summary>
        // #if UNITY_2020_3_OR_NEWER
        //         [UnityEngine.SerializeReference]
        // #endif
        public List<BTNodeDataBase> Nodes = new List<BTNodeDataBase>();
    }
}