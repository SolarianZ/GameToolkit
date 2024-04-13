using System.Collections.Generic;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点数据 - 复合节点。<br/>
    /// 复合节点可以有若干个子节点。没有子节点的复合节点在运行时总是执行失败。
    /// </summary>
    public interface IBTCompositeNodeData : IBTNodeData
    {
        /// <summary>
        /// 子节点GUID列表。<br/>
        /// 运行时按照索引顺序执行子节点。
        /// </summary>
        IReadOnlyList<string> ChildNodeGuids { get; }
    }
}