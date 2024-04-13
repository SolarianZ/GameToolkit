using System;
using System.Collections.Generic;
using GBG.GameToolkit.AI.Common;
using GBG.GameToolkit.AI.Unity.StateMachine;
using UnityEngine;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机节点数据。
    /// </summary>
    [Serializable]
    public abstract class UnitySMNodeDataBase : ISMNodeData
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
        [SerializeReference]
        public List<UnitySMTransitionData> Transitions;


        ISMNodeInstance ISMNodeData.CreateNodeInstance(SMContext context)
        {
            return CreateNodeInstance(context);
        }

        /// <inheritdoc />
        public abstract ISMNodeInstance CreateNodeInstance(SMContext context);
    }
}