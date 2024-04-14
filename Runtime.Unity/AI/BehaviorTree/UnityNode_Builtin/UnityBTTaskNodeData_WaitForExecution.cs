using GBG.GameToolkit.AI.BehaviorTree;

namespace GBG.GameToolkit.Unity.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点数据 - 等待指定的执行次数。
    /// </summary>
    public class UnityBTTaskNodeData_WaitForExecution : UnityBTTaskNodeDataBase
    {
        /// <summary>
        /// 需要等待的执行次数。
        /// </summary>
        public int WaitCount;

        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            BTTaskNodeInstance_WaitForExecution instance = new BTTaskNodeInstance_WaitForExecution(constructInfo, WaitCount);
            return instance;
        }
    }
}
