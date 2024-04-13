using System.Collections.Generic;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    public interface IBTCompositeNodeInstance : IBTNodeInstance
    {
        /// <summary>
        /// 子节点GUID列表。<br/>
        /// 运行时按照索引顺序执行子节点。
        /// </summary>
        internal IReadOnlyList<string> ChildNodeGuids { get; set; }
        /// <summary>
        /// 子节点实例列表。<br/>
        /// 运行时按照索引顺序执行子节点。
        /// </summary>
        internal IReadOnlyList<IBTNodeInstance> ChildNodes { get; set; }
    }
}