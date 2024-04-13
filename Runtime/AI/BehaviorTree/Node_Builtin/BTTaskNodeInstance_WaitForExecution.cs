namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点 - 等待指定的执行次数。
    /// </summary>
    public class BTTaskNodeInstance_WaitForExecution : BTTaskNodeInstance_WaitForExecutionBase
    {
        /// <summary>
        /// 需要等待的执行次数。
        /// </summary>
        private int _waitCount;


        public BTTaskNodeInstance_WaitForExecution(BTNodeConstructInfo constructInfo, int waitCount) : base(constructInfo)
        {
            _waitCount = waitCount;
        }

        /// <inheritdoc/>
        public override int GetWaitCount()
        {
            return _waitCount;
        }
    }
}
