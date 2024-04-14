using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机节点数据。
    /// </summary>
    [Serializable]
    public abstract class SMNodeDataBase : ISMNodeData
    {
        /// <inheritdoc />
        string IUniqueItem.Guid => Guid;
        /// <inheritdoc />
        string ISMNodeData.Name => Name;
        /// <inheritdoc />
        IReadOnlyList<ISMTransitionData> ISMNodeData.Transitions => Transitions;

        /// <summary>
        /// 节点名称。
        /// </summary>
        public string Name;
        /// <summary>
        /// 节点GUID。
        /// </summary>
        // [Readonly]
        public string Guid;
        /// <summary>
        /// 节点可执行的转换。
        /// </summary>
        public List<SMTransitionData> Transitions;


        ISMNodeInstance ISMNodeData.CreateNodeInstance(SMContext context)
        {
            return CreateNodeInstance(context);
        }

        /// <inheritdoc />
        public abstract ISMNodeInstance CreateNodeInstance(SMContext context);
    }
}