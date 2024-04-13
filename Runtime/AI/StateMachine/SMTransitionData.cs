using System;
using GBG.GameToolkit.AI.Condition;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机状态转换数据。
    /// </summary>
    [Serializable]
    public class SMTransitionData : ConditionGroupData, ISMTransitionData
    {
        /// <inheritdoc />
        string ISMTransitionData.DestNodeGuid => DestNodeGuid;
        
        /// <summary>
        /// 目标节点GUID。
        /// </summary>
        public string DestNodeGuid;
    }
}