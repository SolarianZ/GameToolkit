using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点 - 等待参数所指定的执行次数。
    /// </summary>
    public class BTTaskNodeInstance_WaitForParamExecution : BTTaskNodeInstance_WaitForExecutionBase
    {
        /// <summary>
        /// 需要等待的执行次数参数。
        /// </summary>
        internal IParamInstance WaitCountParam { get; }


        public BTTaskNodeInstance_WaitForParamExecution(BTNodeConstructInfo constructInfo, string paramGuid)
            : base(constructInfo)
        {
            WaitCountParam = Context.Blackboard.GetParamByGuidWithException(paramGuid, ParamType.Int32);
        }

        /// <inheritdoc/>
        public override int GetWaitCount()
        {
            return WaitCountParam.GetInt32();
        }
    }
}
