using System.Collections.Generic;
using GBG.GameToolkit.AI.Condition;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点构造信息。
    /// </summary>
    public struct BTNodeConstructInfo
    {
        /// <summary>
        /// 行为树上下文。
        /// </summary>
        public BTContext Context;
        /// <summary>
        /// 节点GUID。
        /// </summary>
        public string Guid;
        /// <summary>
        /// 节点名称。
        /// </summary>
        public string Name;
        /// <summary>
        /// 节点可执行的前置条件。
        /// </summary>
        public IReadOnlyList<IConditionGroupInstance> PreconditionGroups;
        /// <summary>
        /// 父节点GUID。
        /// </summary>
        public string ParentGuid;
        /// <summary>
        /// 子节点GUID列表。<br/>
        /// 运行时按照索引顺序执行子节点。
        /// </summary>
        public IReadOnlyList<string> ChildGuids;
    }
}
