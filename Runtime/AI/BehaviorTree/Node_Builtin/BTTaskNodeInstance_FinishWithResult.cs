namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点 - 返回指定的执行结果。
    /// </summary>
    public class BTTaskNodeInstance_FinishWithResult : BTTaskNodeInstanceBase
    {
        /// <summary>
        /// 执行结果。
        /// </summary>
        internal BTNodeResult Result { get; }

        public BTTaskNodeInstance_FinishWithResult(BTNodeConstructInfo constructInfo, BTNodeResult result)
            : base(constructInfo)
        {
            Result = result;
        }

        /// <inheritdoc/>
        protected override BTNodeResult ExecuteTask()
        {
            return Result;
        }
    }
}
