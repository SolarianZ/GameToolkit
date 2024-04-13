namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点执行结果。
    /// </summary>
    public enum BTNodeResult : byte
    {
        /// <summary>
        /// 执行中。
        /// </summary>
        InProgress = 0,
        /// <summary>
        /// 执行成功。
        /// </summary>
        Success = 1,
        /// <summary>
        /// 执行失败。
        /// </summary>
        Failure = 2,
    }
}
