using System.Collections.Generic;
using GBG.GameToolkit.AI.BehaviorTree;
using GBG.GameToolkit.AI.Parameter;
using UnityEngine;

namespace GBG.GameToolkit.AI.Unity.BehaviorTree
{
    [CreateAssetMenu(menuName = "Test/AI/Behavior Tree Asset")]
    public class UnityBehaviorTreeAsset : ScriptableObject, IBehaviorTreeData
    {
        /// <inheritdoc />
        public string Comment => ((IBehaviorTreeData)Data).Comment;
        /// <inheritdoc />
        public IBlackboardData Blackboard => ((IBehaviorTreeData)Data).Blackboard;
        /// <inheritdoc />
        public bool AlwaysExecuteFromRoot => ((IBehaviorTreeData)Data).AlwaysExecuteFromRoot;
        /// <inheritdoc />
        public string RootNodeGuid => ((IBehaviorTreeData)Data).RootNodeGuid;
        /// <inheritdoc />
        public IReadOnlyList<IBTNodeData> Nodes => ((IBehaviorTreeData)Data).Nodes;

        public UnityBehaviorTreeData Data;
    }
}