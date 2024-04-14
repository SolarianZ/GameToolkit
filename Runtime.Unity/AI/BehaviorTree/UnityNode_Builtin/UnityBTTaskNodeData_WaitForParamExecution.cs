using System;
using GBG.GameToolkit.AI.BehaviorTree;

namespace GBG.GameToolkit.Unity.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点数据 - 等待参数所指定的执行次数。
    /// </summary>
    [Serializable]
    public class UnityBTTaskNodeData_WaitForParamExecution : UnityBTTaskNodeDataBase
    {
        /// <summary>
        /// 需要等待的执行次数参数GUID。
        /// </summary>
        public string WaitExecutionParamGuid;


        /// <inheritdoc/>
        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            BTTaskNodeInstance_WaitForParamExecution instance = new BTTaskNodeInstance_WaitForParamExecution(constructInfo, WaitExecutionParamGuid);
            return instance;
        }
    }
}
