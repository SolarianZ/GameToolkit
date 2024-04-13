using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树上下文。
    /// </summary>
    public class BTContext
    {
        /// <summary>
        /// 执行序号。<br/>
        /// 每次调用<see cref="BehaviorTreeInstance.Evaluate(float)"/>时递增。
        /// </summary>
        public long ExecutionId { get; internal set; }
        /// <summary>
        /// 已执行时间（秒）。
        /// </summary>
        public double ExecutionTime { get; internal set; }
        /// <summary>
        /// 执行时间增量（秒）。
        /// </summary>
        public float ExecutionDeltaTime { get; internal set; }
        /// <summary>
        /// 行为树描述信息。
        /// </summary>
        public string BTComment { get; internal set; }
        /// <summary>
        /// 黑板。
        /// </summary>
        public IBlackboardInstance Blackboard { get; internal set; }
        /// <summary>
        /// 行为树拥有者。
        /// </summary>
        public object Owner { get; internal set; }
        /// <summary>
        /// 用户数据。
        /// </summary>
        public object UserData { get; internal set; }
        /// <summary>
        /// 是否在被动模式下执行行为树。
        /// </summary>
        /// <seealso cref="PassiveExecutionNode"/>
        /// <seealso cref="BehaviorTreeData.AlwaysExecuteFromRoot"/>
        /// <seealso cref="BTNodeInstanceBase.GetNextPassiveExecutionNode"/>
        public bool ExecuteInPassiveMode { get; set; }
        /// <summary>
        /// 被动执行模式下的目标执行节点。
        /// </summary>
        internal IBTNodeInstance PassiveExecutionNode { get; set; }
    }
}
