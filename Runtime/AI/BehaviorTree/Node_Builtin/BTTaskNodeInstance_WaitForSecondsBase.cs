namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点 - 等待时间。
    /// </summary>
    public abstract class BTTaskNodeInstance_WaitForSecondsBase : BTTaskNodeInstanceBase
    {
        /// <summary>
        /// 剩余需要等待的时间（秒）。
        /// </summary>
        public float RemainingTime { get; private set; }


        protected BTTaskNodeInstance_WaitForSecondsBase(BTNodeConstructInfo constructInfo) : base(constructInfo)
        {
        }

        /// <summary>
        /// 获取需要等待的时间（秒）。
        /// </summary>
        /// <returns></returns>
        public abstract float GetWaitTime();

        /// <inheritdoc/>
        protected override BTNodeResult ExecuteTask()
        {
            if (!((IBTNodeInstance)this).IsInProgress)
            {
                RemainingTime = GetWaitTime();
            }

            RemainingTime -= Context.ExecutionDeltaTime;
            if (RemainingTime > 0)
            {
                return BTNodeResult.InProgress;
            }

            return BTNodeResult.Success;
        }
    }
}