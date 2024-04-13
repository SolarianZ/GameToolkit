using GBG.GameToolkit.AI.Common;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树复合节点 - 选择器节点。<br/>
    /// 在运行时，选择器会按索引顺序执行子节点。
    /// 当遇到首个返回<see cref="BTNodeResult.Success"/>或<see cref="BTNodeResult.InProgress"/>的子节点时，自身返回<see cref="BTNodeResult.Success"/>，并且停止执行其他子节点。<br/>
    /// 若所有子节点都返回<see cref="BTNodeResult.Failure"/>，自身返回<see cref="BTNodeResult.Failure"/>。
    /// </summary>
    public class BTCompositeNodeInstance_Selector : BTCompositeNodeInstanceBase
    {
        /// <summary>
        /// 正在执行的子节点。
        /// </summary>
        private IBTNodeInstance _inProgressChildNode;


        public BTCompositeNodeInstance_Selector(BTNodeConstructInfo constructInfo) : base(constructInfo)
        {
        }

        /// <inheritdoc/>
        protected override void OnAborted()
        {
            base.OnAborted();

            _inProgressChildNode = null;
        }

        /// <inheritdoc/>
        protected override BTNodeResult ExecuteImpl()
        {
            if (ChildNodes == null)
            {
                return BTNodeResult.Failure;
            }

            // 在主动执行模式下，每次都要重新选择活动子节点，所以可能要中断上次执行时选中的活动子节点
            bool abortRightSiblings = false;
            BTNodeResult selfResult = BTNodeResult.Failure;
            _inProgressChildNode = null;
            for (int i = 0; i < ChildNodes.Count; i++)
            {
                IBTNodeInstance childNode = ChildNodes[i];
                if (abortRightSiblings)
                {
                    childNode.Abort();
                    continue;
                }

                BTNodeResult childResult = childNode.Execute();
                if (childResult == BTNodeResult.InProgress || childResult == BTNodeResult.Success)
                {
                    selfResult = BTNodeResult.Success;
                    abortRightSiblings = true;
                }

                if (childResult == BTNodeResult.InProgress)
                {
                    _inProgressChildNode = childNode;
                }
            }

            return selfResult;
        }

        /// <inheritdoc/>
        internal override IBTNodeInstance GetNextPassiveExecutionNode()
        {
            if (_inProgressChildNode != null)
            {
                Debugger.Assert(_inProgressChildNode.LastExecutionResult == BTNodeResult.InProgress,
                    $"The in-progress child node should be in progress. Child node guid: {_inProgressChildNode.Guid}.");
                return _inProgressChildNode;
            }

            return ParentNode?.GetNextPassiveExecutionNode();
        }
    }
}