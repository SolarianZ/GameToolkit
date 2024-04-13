using System;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点数据 - 等待参数所指定的时间。
    /// </summary>
    [Serializable]
    public class BTTaskNodeData_WaitForParamSeconds : BTTaskNodeDataBase
    {
        /// <summary>
        /// 需要等待的时间参数GUID。
        /// </summary>
        public string WaitTimeParamGuid;


        /// <inheritdoc/>
        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            BTTaskNodeInstance_WaitForParamSeconds instance = new BTTaskNodeInstance_WaitForParamSeconds(constructInfo, WaitTimeParamGuid);
            return instance;
        }
    }
}
