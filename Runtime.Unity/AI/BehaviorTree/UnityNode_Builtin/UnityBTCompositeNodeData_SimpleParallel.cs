using System;
using GBG.GameToolkit.AI.BehaviorTree;

namespace GBG.GameToolkit.Unity.AI.BehaviorTree
{
    /// <summary>
    /// 行为树复合节点数据 - 简单并行节点。<br/>
    /// 同时执行所有子节点。子节点只能是任务节点（<see cref="BTTaskNodeDataBase"/>）。<br/>
    /// 执行结果的计算方式取决于<see cref="FinishCondition"/>。
    /// </summary>
    /// <seealso cref="BTSimpleParallelFinishCondition"/>
    [Serializable]
    public class UnityBTCompositeNodeData_SimpleParallel : UnityBTCompositeNodeDataBase
    {
        /// <summary>
        /// 节点执行结束条件。
        /// </summary>
        public BTSimpleParallelFinishCondition FinishCondition = BTSimpleParallelFinishCondition.FirstBranchFinish;

        /// <inheritdoc/>
        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            BTCompositeNodeInstance_SimpleParallel instance = new BTCompositeNodeInstance_SimpleParallel(constructInfo, FinishCondition);
            return instance;
        }
    }
}
