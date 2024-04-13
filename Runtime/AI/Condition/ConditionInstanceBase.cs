using System;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件。
    /// </summary>
    public abstract class ConditionInstanceBase : IConditionInstance
    {
        /// <summary>
        /// 黑板。
        /// </summary>
        protected IBlackboardInstance Blackboard { get; }
        /// <inheritdoc/>
        public abstract bool UseEvaluationResultCache { get; }

        /// <inheritdoc/>
        public event Action OnEvaluationResultCacheDirty;


        protected ConditionInstanceBase(IBlackboardInstance blackboard)
        {
            Blackboard = blackboard;
        }

        /// <inheritdoc/>
        public abstract bool Evaluate();

        /// <inheritdoc/>
        // ReSharper disable once MemberHidesInterfaceMemberWithDefaultImplementation
        protected void SetEvaluationResultCacheDirty()
        {
            ((IConditionInstance)this).SetEvaluationResultCacheDirty();
        }

        /// <inheritdoc />
        void IConditionInstance.RaiseEvaluationResultCacheDirtyEvent()
        {
            OnEvaluationResultCacheDirty?.Invoke();
        }
    }
}