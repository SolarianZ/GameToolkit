﻿using System;
using System.Collections.Generic;
using GBG.GameToolkit.AI.BehaviorTree;

namespace GBG.GameToolkit.Unity.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点数据 - 复合节点。<br/>
    /// 复合节点可以有若干个子节点。没有子节点的复合节点在运行时总是执行失败。
    /// </summary>
    [Serializable]
    public abstract class UnityBTCompositeNodeDataBase : UnityBTNodeDataBase, IBTCompositeNodeData
    {
        /// <inheritdoc />
        IReadOnlyList<string> IBTCompositeNodeData.ChildNodeGuids => ChildNodeGuids;

        public List<string> ChildNodeGuids = new List<string>();
    }
}