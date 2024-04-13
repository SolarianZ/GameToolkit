using System.Collections.Generic;
using GBG.GameToolkit.AI.Condition;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树帮助类。
    /// </summary>
    public static class BehaviorTreeHelper
    {
        /// <summary>
        /// 连接并初始化节点。
        /// </summary>
        /// <param name="nodeGuidTable">节点GUID-节点实例映射表。</param>
        internal static void ConnectAndInitializeNodeInstances(IReadOnlyDictionary<string, IBTNodeInstance> nodeGuidTable)
        {
            foreach (IBTNodeInstance node in nodeGuidTable.Values)
            {
                // Parent node
                if (!string.IsNullOrEmpty(node.ParentNodeGuid))
                {
                    node.ParentNode = nodeGuidTable[node.ParentNodeGuid];
                    //node.ParentNodeGuid = null;
                }

                // Child node
                if (node is IBTCompositeNodeInstance compositeNode && compositeNode.ChildNodeGuids != null && compositeNode.ChildNodeGuids.Count > 0)
                {
                    IBTNodeInstance[] childNodes = new IBTNodeInstance[compositeNode.ChildNodeGuids.Count];
                    for (int i = 0; i < compositeNode.ChildNodeGuids.Count; i++)
                    {
                        IBTNodeInstance childNode = nodeGuidTable[compositeNode.ChildNodeGuids[i]];
                        childNodes[i] = childNode;
                    }

                    compositeNode.ChildNodes = childNodes;
                    //compositeNode.ChildNodeGuids = null;
                }

                // Initialize
                node.Initialize();
            }
        }

        /// <summary>
        /// 创建节点构造信息。
        /// </summary>
        /// <param name="nodeData">节点数据。</param>
        /// <param name="context">行为树上下文。</param>
        /// <returns></returns>
        public static BTNodeConstructInfo CreateNodeConstructInfo(this IBTNodeData nodeData, BTContext context)
        {
            return new BTNodeConstructInfo
            {
                Context = context,
                Guid = nodeData.Guid,
                Name = nodeData.Name,
                PreconditionGroups = nodeData.CreatePreconditionGroupInstances(context.Blackboard),
                ParentGuid = nodeData.ParentGuid,
                ChildGuids = (nodeData as IBTCompositeNodeData)?.ChildNodeGuids,
            };
        }

        /// <summary>
        /// 创建节点可执行的前置条件实例。
        /// </summary>
        /// <param name="nodeData">节点数据。</param>
        /// <param name="blackboard">黑板。</param>
        /// <returns></returns>
        private static IConditionGroupInstance[] CreatePreconditionGroupInstances(this IBTNodeData nodeData, IBlackboardInstance blackboard)
        {
            IConditionGroupInstance[] conditionGroupInstances = null;
            if (nodeData.PreconditionGroups != null)
            {
                conditionGroupInstances = new IConditionGroupInstance[nodeData.PreconditionGroups.Count];
                for (int i = 0; i < nodeData.PreconditionGroups.Count; i++)
                {
                    IConditionGroupData conditionGroupData = nodeData.PreconditionGroups[i];
                    IConditionGroupInstance conditionGroupInstance = new ConditionGroupInstance(blackboard, conditionGroupData);
                    conditionGroupInstances[i] = conditionGroupInstance;
                }
            }

            return conditionGroupInstances;
        }
    }
}