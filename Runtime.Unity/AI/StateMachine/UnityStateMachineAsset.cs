using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;
using GBG.GameToolkit.AI.StateMachine;
using UnityEngine;

namespace GBG.GameToolkit.Unity.AI.StateMachine
{
    [CreateAssetMenu(menuName = "Bamboo/AI/State Machine Asset")]
    public class UnityStateMachineAsset : ScriptableObject, IStateMachineData
    {
        /// <inheritdoc />
        public string Comment => ((IStateMachineData)Data).Comment;
        /// <inheritdoc />
        public IBlackboardData Blackboard => ((IStateMachineData)Data).Blackboard;
        /// <inheritdoc />
        public int MaxTransitionCountPerEvaluation => ((IStateMachineData)Data).MaxTransitionCountPerEvaluation;
        /// <inheritdoc />
        public string EntryNodeGuid => ((IStateMachineData)Data).EntryNodeGuid;
        /// <inheritdoc />
        public IReadOnlyList<ISMNodeData> Nodes => ((IStateMachineData)Data).Nodes;

        public UnityStateMachineData Data;
    }
}