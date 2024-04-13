using System;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节复合点数据 - 选择器节点。<br/>
    /// 在运行时，选择器节点会按索引顺序执行子节点。
    /// 当遇到首个返回<see cref="BTNodeResult.Success"/>或<see cref="BTNodeResult.InProgress"/>的子节点时，自身返回<see cref="BTNodeResult.Success"/>，并且停止执行其他子节点。<br/>
    /// 若没有子节点，或者所有子节点都返回<see cref="BTNodeResult.Failure"/>，自身返回<see cref="BTNodeResult.Failure"/>。
    /// </summary>
    [Serializable]
    public class BTCompositeNodeData_Selector : BTCompositeNodeDataBase
    {
        /// <inheritdoc/>
        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            BTCompositeNodeInstance_Selector instance = new BTCompositeNodeInstance_Selector(constructInfo);
            return instance;
        }
    }
}
