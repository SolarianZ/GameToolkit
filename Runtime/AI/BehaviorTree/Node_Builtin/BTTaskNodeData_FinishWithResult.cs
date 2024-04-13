using System;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点数据 - 返回指定的执行结果。
    /// </summary>
    [Serializable]
    public class BTTaskNodeData_FinishWithResult : BTTaskNodeDataBase
    {
        /// <summary>
        /// 执行结果。
        /// </summary>
        public BTNodeResult Result;


        /// <inheritdoc/>
        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            BTTaskNodeInstance_FinishWithResult instance = new BTTaskNodeInstance_FinishWithResult(constructInfo, Result);
            return instance;
        }
    }
}
