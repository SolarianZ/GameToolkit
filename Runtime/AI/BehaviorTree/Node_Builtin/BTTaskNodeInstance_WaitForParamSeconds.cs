using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点 - 等待参数所指定的时间。
    /// </summary>
    public class BTTaskNodeInstance_WaitForParamSeconds : BTTaskNodeInstance_WaitForSecondsBase
    {
        /// <summary>
        /// 需要等待的时间参数（秒）。
        /// </summary>
        internal IParamInstance WaitTimeParam { get; }


        public BTTaskNodeInstance_WaitForParamSeconds(BTNodeConstructInfo constructInfo, string paramGuid)
            : base(constructInfo)
        {
            WaitTimeParam = Context.Blackboard.GetParamByGuidWithException(paramGuid, ParamType.Float32, ParamType.Int32);
        }

        /// <inheritdoc/>
        public override float GetWaitTime()
        {
            if (WaitTimeParam.Type == ParamType.Int32)
                return WaitTimeParam.GetInt32();

            return WaitTimeParam.GetFloat32();
        }
    }
}
