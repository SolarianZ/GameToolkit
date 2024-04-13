using System;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机节点数据 - 空状态。
    /// </summary>
    [Serializable]
    public class SMNodeData_Dummy : SMNodeDataBase
    {
        /// <inheritdoc/>
        public override ISMNodeInstance CreateNodeInstance(SMContext context)
        {
            SMNodeInfo nodeInfo = this.CreateNodeInfo(context.Blackboard);
            return new SMNodeInstance_Dummy(context, nodeInfo);
        }
    }
}