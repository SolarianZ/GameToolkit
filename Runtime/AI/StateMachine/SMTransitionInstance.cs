using GBG.GameToolkit.AI.Condition;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机状态转换。
    /// </summary>
    public class SMTransitionInstance : ConditionGroupInstance, ISMTransitionInstance
    {
        /// <inheritdoc/>
        public string DestNodeGuid { get; }


        public SMTransitionInstance(IBlackboardInstance blackboard, ISMTransitionData data)
            : base(blackboard, data)
        {
            DestNodeGuid = data.DestNodeGuid;
        }
    }
}