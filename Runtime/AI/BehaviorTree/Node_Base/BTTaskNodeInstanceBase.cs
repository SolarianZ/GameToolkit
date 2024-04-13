namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点。<br/>
    /// 任务节点是行为树的叶子节点，用于执行具体的行为逻辑。
    /// </summary>
    public abstract class BTTaskNodeInstanceBase : BTNodeInstanceBase
    {
        protected BTTaskNodeInstanceBase(BTNodeConstructInfo constructInfo) : base(constructInfo)
        {
        }

        /// <inheritdoc/>
        protected sealed override BTNodeResult ExecuteImpl()
        {
            BTNodeResult taskResult = ExecuteTask();

            return taskResult;
        }

        /// <summary>
        /// 执行节点。
        /// </summary>
        /// <returns>节点执行结果。</returns>
        protected abstract BTNodeResult ExecuteTask();

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