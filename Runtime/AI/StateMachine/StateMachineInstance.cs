using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机。
    /// </summary>
    public class StateMachineInstance
    {
        /// <summary>
        /// 状态机上下文。
        /// </summary>
        public SMContext Context { get; }
        /// <summary>
        /// 入口节点。
        /// </summary>
        internal ISMNodeInstance EntryNode { get; }
        /// <summary>
        /// 当前执行节点。
        /// </summary>
        internal ISMNodeInstance CurrentNode { get; private set; }
        /// <summary>
        /// 节点GUID-节点实例映射表。
        /// </summary>
        internal IReadOnlyDictionary<string, ISMNodeInstance> NodeGuidTable { get; }


        public StateMachineInstance(IStateMachineData smData, object owner, object userData = null)
        {
            Context = new SMContext
            {
                Blackboard = new BlackboardInstance(smData.Blackboard),
                MaxTransitionCountPerEvaluation = smData.MaxTransitionCountPerEvaluation,
                Owner = owner,
                UserData = userData,
            };
            NodeGuidTable = CreateSMNodeInstances(smData, Context);
            NodeGuidTable.TryGetValue(smData.EntryNodeGuid, out ISMNodeInstance entryNode);
            EntryNode = entryNode;
            if (EntryNode == null)
            {
                Debugger.Log(LogLevel.ERROR, "State Machine", $"Entry node not found. Comment: {Context.SMComment}", this);
            }

            Initialize();
        }

        private Dictionary<string, ISMNodeInstance> CreateSMNodeInstances(IStateMachineData smData, SMContext context)
        {
            Dictionary<string, ISMNodeInstance> nodeGuidTable = new Dictionary<string, ISMNodeInstance>(smData.Nodes.Count);
            for (int i = 0; i < smData.Nodes.Count; i++)
            {
                ISMNodeInstance nodeInstance = smData.Nodes[i].CreateNodeInstance(context);
                nodeGuidTable.Add(nodeInstance.Info.Guid, nodeInstance);
            }

            return nodeGuidTable;
        }

        private void Initialize()
        {
            foreach (ISMNodeInstance node in NodeGuidTable.Values)
            {
                node.Initialize();
            }
        }

        /// <summary>
        /// 重置状态机。
        /// </summary>
        public void Reset()
        {
            Context.Reset();
            CurrentNode = null;

            foreach (ISMNodeInstance node in NodeGuidTable.Values)
            {
                node.Reset();
            }
        }

        /// <summary>
        /// 评估状态机。
        /// </summary>
        /// <param name="deltaTime">时间增量（秒）。</param>
        public void Evaluate(float deltaTime)
        {
            Context.ExecutionId++;
            Context.ExecutionTime += deltaTime;
            Context.ExecutionDeltaTime = deltaTime;

            if (EntryNode == null)
            {
                return;
            }

            CurrentNode ??= EntryNode; // 首次执行

            int maxTransitionCount = Context.MaxTransitionCountPerEvaluation > 0
                ? Context.MaxTransitionCountPerEvaluation
                : int.MaxValue;
            int executedTransitionCount = 0;
            while (executedTransitionCount < maxTransitionCount)
            {
                if (!Context.CurrentNodeEntered)
                {
                    Context.CurrentNodeEntered = true;
                    CurrentNode.Enter();
                }

                CurrentNode.Execute();

                string destNodeGuid = CurrentNode.FindNextExecutableNodeGuid();
                if (string.IsNullOrEmpty(destNodeGuid))
                {
                    break;
                }

                ISMNodeInstance destNode = NodeGuidTable[destNodeGuid];
                Context.DestNodeInfo = destNode.Info;
                CurrentNode.Exit();
                Context.DestNodeInfo = null;
                Context.SrcNodeInfo = CurrentNode.Info;
                CurrentNode = destNode;
                Context.CurrentNodeEntered = false;

                executedTransitionCount++;
            }
        }
    }
}