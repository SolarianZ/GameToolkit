using System.Collections.Generic;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点 - 复合节点。
    /// </summary>
    public abstract class BTCompositeNodeInstanceBase : BTNodeInstanceBase, IBTCompositeNodeInstance
    {
        /// <inheritdoc />
        IReadOnlyList<string> IBTCompositeNodeInstance.ChildNodeGuids { get; set; }
        /// <inheritdoc />
        IReadOnlyList<IBTNodeInstance> IBTCompositeNodeInstance.ChildNodes
        {
            get => ChildNodes;
            set => ChildNodes = value;
        }
        protected IReadOnlyList<IBTNodeInstance> ChildNodes { get; private set; }

        protected BTCompositeNodeInstanceBase(BTNodeConstructInfo constructInfo) : base(constructInfo)
        {
            ((IBTCompositeNodeInstance)this).ChildNodeGuids = constructInfo.ChildGuids;
        }
    }
}