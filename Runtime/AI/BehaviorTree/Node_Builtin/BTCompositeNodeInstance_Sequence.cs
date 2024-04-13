using System;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树复合节点 - 顺序节点。<br/>
    /// 在运行时，顺序节点会按索引顺序执行子节点。<br/>
    /// 当正在执行的子节点返回<see cref="BTNodeResult.InProgress"/>时，自身返回<see cref="BTNodeResult.InProgress"/>。<br/>
    /// 当正在执行的子节点返回<see cref="BTNodeResult.Success"/>时，开始执行下一个子节点。<br/>
    /// 当正在执行的子节点返回<see cref="BTNodeResult.Failure"/>时，不再执行其他子节点，自身返回<see cref="BTNodeResult.Failure"/>。<br/>
    /// 所有子节点都返回<see cref="BTNodeResult.Success"/>时，自身返回<see cref="BTNodeResult.Success"/>。<br/>
    /// 若没有子节点，自身返回<see cref="BTNodeResult.Failure"/>。
    /// </summary>
    public class BTCompositeNodeInstance_Sequence : BTCompositeNodeInstanceBase
    {
        /// <summary>
        /// 活动子节点索引。<br/>
        /// -1表示无活动子节点。
        /// </summary>
        internal int ActiveChildIndex { get; private set; } = -1;


        public BTCompositeNodeInstance_Sequence(BTNodeConstructInfo constructInfo) : base(constructInfo)
        {
        }

        /// <inheritdoc/>
        protected override void OnAborted()
        {
            base.OnAborted();

            ActiveChildIndex = -1;
        }

        /// <inheritdoc/>
        protected override BTNodeResult ExecuteImpl()
        {
            if (ChildNodes == null || ChildNodes.Count == 0)
            {
                return BTNodeResult.Failure;
            }

            if (ActiveChildIndex < 0)
            {
                ActiveChildIndex = 0;
            }

            BTNodeResult selfResult = BTNodeResult.InProgress;
            while (ActiveChildIndex < ChildNodes.Count)
            {
                IBTNodeInstance childNode = ChildNodes[ActiveChildIndex];
                BTNodeResult childNodeResult = childNode.Execute();
                switch (childNodeResult)
                {
                    case BTNodeResult.InProgress:
                        selfResult = BTNodeResult.InProgress;
                        break;
                    case BTNodeResult.Success:
                        ActiveChildIndex++;
                        selfResult = ActiveChildIndex < ChildNodes.Count
                            ? BTNodeResult.InProgress
                            : BTNodeResult.Success;
                        break;
                    case BTNodeResult.Failure:
                        selfResult = BTNodeResult.Failure;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(childNodeResult), childNodeResult, null);
                }

                if (selfResult != BTNodeResult.Success)
                {
                    break;
                }
            }

            if (selfResult != BTNodeResult.InProgress)
            {
                ActiveChildIndex = -1;
            }

            return selfResult;
        }

        /// <inheritdoc/>
        internal override IBTNodeInstance GetNextPassiveExecutionNode()
        {
            if (ActiveChildIndex > 0 && ActiveChildIndex < ChildNodes.Count)
            {
                return ChildNodes[ActiveChildIndex];
            }

            return ParentNode?.GetNextPassiveExecutionNode();
        }
    }
}
