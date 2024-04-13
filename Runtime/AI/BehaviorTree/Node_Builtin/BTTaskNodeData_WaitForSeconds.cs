using System;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点数据 - 等待指定的时间。
    /// </summary>
    [Serializable]
    public class BTTaskNodeData_WaitForSeconds : BTTaskNodeDataBase
    {
        /// <summary>
        /// 需要等待的时间（秒）。
        /// </summary>
        public float WaitTime;


        /// <inheritdoc/>
        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            BTTaskNodeInstance_WaitForSeconds instance = new BTTaskNodeInstance_WaitForSeconds(constructInfo, WaitTime);
            return instance;
        }
    }
}
