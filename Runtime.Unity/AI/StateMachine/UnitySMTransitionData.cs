using System;
using GBG.GameToolkit.AI.StateMachine;
using GBG.GameToolkit.Unity.AI.Condition;

namespace GBG.GameToolkit.Unity.AI.StateMachine
{
    /// <summary>
    /// 状态机状态转换数据。
    /// </summary>
    [Serializable]
    public class UnitySMTransitionData : UnityConditionGroupData, ISMTransitionData
    {
        /// <inheritdoc />
        string ISMTransitionData.DestNodeGuid => DestNodeGuid;
        
        /// <summary>
        /// 目标节点GUID。
        /// </summary>
        public string DestNodeGuid;
    }
}