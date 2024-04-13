using System;
using System.Collections.Generic;
using GBG.GameToolkit.AI.BehaviorTree;
using GBG.GameToolkit.AI.Common;
using GBG.GameToolkit.AI.Condition;
using GBG.GameToolkit.AI.Unity.Condition;
using UnityEngine;

namespace GBG.GameToolkit.AI.Unity.BehaviorTree
{
    /// <summary>
    /// 行为树节点数据。
    /// </summary>
    [Serializable]
    public abstract class UnityBTNodeDataBase : IBTNodeData
    {
        /// <inheritdoc/>
        string IUniqueItem.Guid => Guid;
        /// <inheritdoc />
        string IBTNodeData.Name => Name;
        /// <inheritdoc />
        string IBTNodeData.ParentGuid => ParentGuid;
        /// <inheritdoc />
        IReadOnlyList<IConditionGroupData> IBTNodeData.PreconditionGroups => PreconditionGroups;

        public string Name;
        // [Readonly]
        public string Guid;
        // [Readonly]
        public string ParentGuid;
        [SerializeReference]
        public List<UnityConditionGroupData> PreconditionGroups;


        /// <inheritdoc />
        IBTNodeInstance IBTNodeData.CreateNodeInstance(BTContext context)
        {
            return CreateNodeInstance(context);
        }

        /// <summary>
        /// 构造此节点实例。
        /// </summary>
        /// <param name="context">行为树上下文。</param>
        /// <returns></returns>
        public abstract IBTNodeInstance CreateNodeInstance(BTContext context);
    }
}