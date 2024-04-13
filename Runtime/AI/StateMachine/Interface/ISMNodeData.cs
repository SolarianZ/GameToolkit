using System.Collections.Generic;
using GBG.GameToolkit.AI.Common;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机节点数据。
    /// </summary>
    public interface ISMNodeData : IUniqueItem
    {
        string Name { get; }
        IReadOnlyList<ISMTransitionData> Transitions { get; }

        /// <summary>
        /// 创建此节点实例。
        /// </summary>
        /// <param name="context">状态机上下文。</param>
        /// <returns></returns>
        ISMNodeInstance CreateNodeInstance(SMContext context);
    }
}