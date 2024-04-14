using System;
using GBG.GameToolkit.AI.BehaviorTree;

namespace GBG.GameToolkit.Unity.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节复合点数据 - 顺序节点。<br/>
    /// 在运行时，顺序节点会按索引顺序执行子节点。<br/>
    /// 当正在执行的子节点返回<see cref="BTNodeResult.InProgress"/>时，自身返回<see cref="BTNodeResult.InProgress"/>。<br/>
    /// 当正在执行的子节点返回<see cref="BTNodeResult.Success"/>时，开始执行下一个子节点。<br/>
    /// 当正在执行的子节点返回<see cref="BTNodeResult.Failure"/>时，不再执行其他子节点，自身返回<see cref="BTNodeResult.Failure"/>。<br/>
    /// 所有子节点都返回<see cref="BTNodeResult.Success"/>时，自身返回<see cref="BTNodeResult.Success"/>。<br/>
    /// 若没有子节点，自身返回<see cref="BTNodeResult.Failure"/>。
    /// </summary>
    [Serializable]
    public class UnityBTCompositeNodeData_Sequence : UnityBTCompositeNodeDataBase
    {
        /// <inheritdoc/>
        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            BTCompositeNodeInstance_Sequence instance = new BTCompositeNodeInstance_Sequence(constructInfo);
            return instance;
        }
    }
}
