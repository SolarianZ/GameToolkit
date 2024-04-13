using System;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树简单并行节点的结束条件。
    /// </summary>
    public enum BTSimpleParallelFinishCondition
    {
        /// <summary>
        /// 当首个子节点结束时，整个并行节点结束，停止执行其他子节点，返回结果与首个子节点一致。
        /// </summary>
        FirstBranchFinish,
        /// <summary>
        /// 所有子节点均返回<see cref="BTNodeResult.Success"/>时，整个并行节点结束，返回<see cref="BTNodeResult.Success"/>。<br/>
        /// 当任一子节点返回<see cref="BTNodeResult.Failure"/>时，整个并行节点结束，停止执行其他子节点，返回<see cref="BTNodeResult.Failure"/>。
        /// </summary>
        AllBranchSuccess,
        /// <summary>
        /// 当任一子节点返回<see cref="BTNodeResult.Success"/>时，整个并行节点结束，停止执行其他子节点，返回<see cref="BTNodeResult.Success"/>。<br/>
        /// 所有子节点均返回<see cref="BTNodeResult.Failure"/>，整个并行节点结束，返回<see cref="BTNodeResult.Failure"/>。
        /// </summary>
        AnyBranchSuccess,
    }

    /// <summary>
    /// 行为树复合节点数据 - 简单并行节点。<br/>
    /// 同时执行所有子节点。子节点只能是任务节点（<see cref="BTTaskNodeDataBase"/>）。<br/>
    /// 执行结果的计算方式取决于<see cref="FinishCondition"/>。
    /// </summary>
    /// <seealso cref="BTSimpleParallelFinishCondition"/>
    [Serializable]
    public class BTCompositeNodeData_SimpleParallel : BTCompositeNodeDataBase
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
