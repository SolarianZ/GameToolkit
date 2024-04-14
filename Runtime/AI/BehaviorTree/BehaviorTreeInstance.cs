using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树。
    /// </summary>
    public class BehaviorTreeInstance
    {
        /// <summary>
        /// 行为树上下文。
        /// </summary>
        public BTContext Context { get; }
        /// <summary>
        /// 根节点。
        /// </summary>
        internal IBTNodeInstance RootNode { get; }
        /// <summary>
        /// 节点GUID-节点实例映射表。
        /// </summary>
        internal IReadOnlyDictionary<string, IBTNodeInstance> NodeGuidTable { get; }


        internal BehaviorTreeInstance(IBehaviorTreeData btData, object owner, object userData = null)
        {
            Context = new BTContext
            {
                BTComment = btData.Comment,
                ExecuteInPassiveMode = !btData.AlwaysExecuteFromRoot,
                Blackboard = new BlackboardInstance(btData.Blackboard),
                Owner = owner,
                UserData = userData,
            };
            NodeGuidTable = CreateBTNodeInstances(btData, Context);
            NodeGuidTable.TryGetValue(btData.RootNodeGuid, out IBTNodeInstance rootNode);
            RootNode = rootNode;
            if (RootNode == null)
            {
                Debugger.Log(LogLevel.ERROR, "Behavior Tree", $"Root node not found. Comment: {Context.BTComment}", this);
            }

            Initialize();
        }

        /// <summary>
        /// 创建节点GUID-节点实例映射表。
        /// </summary>
        /// <param name="btData">行为树数据。</param>
        /// <param name="context">行为树上下文。</param>
        /// <returns></returns>
        private Dictionary<string, IBTNodeInstance> CreateBTNodeInstances(IBehaviorTreeData btData, BTContext context)
        {
            Dictionary<string, IBTNodeInstance> nodeGuidTable = new Dictionary<string, IBTNodeInstance>(btData.Nodes.Count);
            for (int i = 0; i < btData.Nodes.Count; i++)
            {
                IBTNodeInstance nodeInstance = btData.Nodes[i].CreateNodeInstance(context);
                nodeGuidTable.Add(nodeInstance.Guid, nodeInstance);
            }

            return nodeGuidTable;
        }

        /// <summary>
        /// 初始化行为树。
        /// </summary>
        private void Initialize()
        {
            BehaviorTreeHelper.ConnectAndInitializeNodeInstances(NodeGuidTable);
        }

        /// <summary>
        /// 重置行为树状态。
        /// </summary>
        public void Reset()
        {
            foreach (IBTNodeInstance node in NodeGuidTable.Values)
            {
                node.Reset();
            }
        }

        /// <summary>
        /// 评估行为树。
        /// </summary>
        /// <param name="deltaTime">时间增量（秒）。</param>
        public void Evaluate(float deltaTime)
        {
            Context.ExecutionId++;
            Context.ExecutionTime += deltaTime;
            Context.ExecutionDeltaTime = deltaTime;

            if (RootNode == null)
            {
                return;
            }

            if (Context.ExecuteInPassiveMode)
            {
                Context.PassiveExecutionNode ??= RootNode;
                Context.PassiveExecutionNode.Execute();
                Context.PassiveExecutionNode = Context.PassiveExecutionNode.GetNextPassiveExecutionNode();
            }
            else
            {
                RootNode.Execute();
            }
        }
    }
}