namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点 - 等待执行次数。
    /// </summary>
    public abstract class BTTaskNodeInstance_WaitForExecutionBase : BTTaskNodeInstanceBase
    {
        /// <summary>
        /// 剩余需要等待的次数。
        /// </summary>
        public int RemainingCount { get; private set; }


        protected BTTaskNodeInstance_WaitForExecutionBase(BTNodeConstructInfo constructInfo) : base(constructInfo)
        {
        }

        /// <summary>
        /// 获取需要等待的次数。<br/>
        /// 负数表示无限等待。
        /// </summary>
        /// <returns></returns>
        public abstract int GetWaitCount();

        /// <inheritdoc/>
        protected override BTNodeResult ExecuteTask()
        {
            if (!((IBTNodeInstance)this).IsInProgress)
            {
                RemainingCount = GetWaitCount();
            }

            RemainingCount--;
            if (RemainingCount != 0)
            {
                return BTNodeResult.InProgress;
            }

            return BTNodeResult.Success;
        }
    }
}
