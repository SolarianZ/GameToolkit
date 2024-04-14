using System;
using System.Linq;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树复合节点 - 简单并行节点。<br/>
    /// 同时执行所有子节点。子节点只能是任务节点（<see cref="BTTaskNodeInstanceBase"/>）。<br/>
    /// 执行结果的计算方式取决于<see cref="FinishCondition"/>。
    /// </summary>
    /// <seealso cref="BTSimpleParallelFinishCondition"/>
    public class BTCompositeNodeInstance_SimpleParallel : BTCompositeNodeInstanceBase
    {
        /// <summary>
        /// 节点执行结束条件。
        /// </summary>
        internal BTSimpleParallelFinishCondition FinishCondition { get; }

        public BTCompositeNodeInstance_SimpleParallel(BTNodeConstructInfo constructInfo, BTSimpleParallelFinishCondition finishCondition)
            : base(constructInfo)
        {
            FinishCondition = finishCondition;
        }

        /// <inheritdoc/>
        protected override BTNodeResult ExecuteImpl()
        {
            if (ChildNodes == null || ChildNodes.Count == 0)
            {
                return BTNodeResult.Failure;
            }

            Debugger.Assert(ChildNodes.All(node => node is BTTaskNodeInstanceBase),
                $"Child nodes of {nameof(BTCompositeNodeInstance_SimpleParallel)} node must be task nodes. Node guid: {Guid}.");
            Debugger.Assert(Context.PassiveExecutionNode == this || !(Context.PassiveExecutionNode is BTCompositeNodeInstance_SimpleParallel),
                $"There is already a {nameof(BTCompositeNodeInstance_SimpleParallel)} node running in the context. Node guid: {Guid}.");

            BTNodeResult selfResult;
            switch (FinishCondition)
            {
                case BTSimpleParallelFinishCondition.FirstBranchFinish:
                    selfResult = Execute_FirstBranchSuccess();
                    break;
                case BTSimpleParallelFinishCondition.AllBranchSuccess:
                    selfResult = Execute_AllBranchSuccess();
                    break;
                case BTSimpleParallelFinishCondition.AnyBranchSuccess:
                    selfResult = Execute_AnyBranchSuccess();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(FinishCondition), FinishCondition, null);
            }


            return selfResult;
        }

        private BTNodeResult Execute_FirstBranchSuccess()
        {
            bool isNewExecution = LastExecutionResult != BTNodeResult.InProgress;
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                IBTNodeInstance childNode = ChildNodes[i];
                BTNodeResult childNodeResult;
                if (isNewExecution || childNode.LastExecutionResult == BTNodeResult.InProgress)
                {
                    childNodeResult = childNode.Execute();
                }
                else
                {
                    continue;
                }

                if (i == 0)
                {
                    if (childNodeResult == BTNodeResult.Success || childNodeResult == BTNodeResult.Failure)
                    {
                        AbortAllChildren();
                        return childNodeResult;
                    }
                }
            }

            return BTNodeResult.InProgress;
        }

        private BTNodeResult Execute_AllBranchSuccess()
        {
            bool isNewExecution = LastExecutionResult != BTNodeResult.InProgress;
            bool allChildrenFinished = true;
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                IBTNodeInstance childNode = ChildNodes[i];
                BTNodeResult childNodeResult;
                if (isNewExecution || childNode.LastExecutionResult == BTNodeResult.InProgress)
                {
                    childNodeResult = childNode.Execute();
                }
                else
                {
                    continue;
                }

                if (childNodeResult == BTNodeResult.InProgress)
                {
                    allChildrenFinished = false;
                }
                else if (childNodeResult == BTNodeResult.Failure)
                {
                    AbortAllChildren();
                    return BTNodeResult.Failure;
                }
            }

            if (allChildrenFinished)
            {
                return BTNodeResult.Success;
            }

            return BTNodeResult.InProgress;
        }

        private BTNodeResult Execute_AnyBranchSuccess()
        {
            bool isNewExecution = LastExecutionResult != BTNodeResult.InProgress;
            bool allChildrenFinished = true;
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                IBTNodeInstance childNode = ChildNodes[i];
                BTNodeResult childNodeResult;
                if (isNewExecution || childNode.LastExecutionResult == BTNodeResult.InProgress)
                {
                    childNodeResult = childNode.Execute();
                }
                else
                {
                    continue;
                }

                if (childNodeResult == BTNodeResult.InProgress)
                {
                    allChildrenFinished = false;
                }
                else if (childNodeResult == BTNodeResult.Success)
                {
                    AbortAllChildren();
                    return BTNodeResult.Success;
                }
            }

            if (allChildrenFinished)
            {
                return BTNodeResult.Success;
            }

            return BTNodeResult.InProgress;
        }

        private void AbortAllChildren()
        {
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                IBTNodeInstance childNode = ChildNodes[i];
                childNode.Abort();
            }
        }

        /// <inheritdoc/>
        internal override IBTNodeInstance GetNextPassiveExecutionNode()
        {
            if (LastExecutionResult != BTNodeResult.InProgress)
            {
                return ParentNode?.GetNextPassiveExecutionNode();
            }

            return this;
        }
    }
}