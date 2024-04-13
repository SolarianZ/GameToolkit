using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机上下文。
    /// </summary>
    public class SMContext
    {
        /// <summary>
        /// 执行序号。<br/>
        /// 每次调用<see cref="StateMachineInstance.Evaluate(float)"/>时递增。
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
        /// 状态机描述信息。
        /// </summary>
        public string SMComment { get; internal set; }
        /// <summary>
        /// 黑板。
        /// </summary>
        public IBlackboardInstance Blackboard { get; internal set; }
        /// <summary>
        /// 每次评估状态机时，可执行的状态转换次数上限。
        /// </summary>
        public int MaxTransitionCountPerEvaluation = 1;
        /// <summary>
        /// 状态机拥有者。
        /// </summary>
        public object Owner { get; internal set; }
        /// <summary>
        /// 用户数据。
        /// </summary>
        public object UserData { get; internal set; }
        /// <summary>
        /// 当前状态的源状态节点信息。
        /// </summary>
        public SMNodeInfo SrcNodeInfo { get; internal set; }
        /// <summary>
        /// 当前状态的目标状态节点信息。
        /// </summary>
        public SMNodeInfo DestNodeInfo { get; internal set; }
        /// <summary>
        /// 当前状态是否已进入。
        /// </summary>
        internal bool CurrentNodeEntered { get; set; }


        internal void Reset()
        {
            ExecutionId = 0;
            ExecutionTime = 0;
            ExecutionDeltaTime = 0;
            SrcNodeInfo = null;
            DestNodeInfo = null;
            CurrentNodeEntered = false;
        }
    }
}
