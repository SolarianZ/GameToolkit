using System.Collections.Generic;
using GBG.GameToolkit.AI.Common;
using GBG.GameToolkit.AI.Condition;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点数据。
    /// </summary>
    public interface IBTNodeData : IUniqueItem
    {
        /// <summary>
        /// 节点名称。
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 父节点GUID。
        /// </summary>
        string ParentGuid { get; }
        /// <summary>
        /// 节点可执行的前置条件。
        /// </summary>
        IReadOnlyList<IConditionGroupData> PreconditionGroups { get; }


        /// <summary>
        /// 构造此节点实例。
        /// </summary>
        /// <param name="context">行为树上下文。</param>
        /// <returns></returns>
        IBTNodeInstance CreateNodeInstance(BTContext context);
    }
}