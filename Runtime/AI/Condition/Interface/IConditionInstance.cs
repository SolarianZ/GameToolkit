using System;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件。
    /// </summary>
    public interface IConditionInstance
    {
        /// <summary>
        /// 是否使用评估结果缓存。<br/>
        /// 注意：若使用评估结果缓存，在影响评估结果的数据发生变化时，必须手动调用<see cref="SetEvaluationResultCacheDirty"/>方法。
        /// </summary>
        bool UseEvaluationResultCache { get; }

        /// <summary>
        /// 评估结果缓存过期事件。
        /// </summary>
        event Action OnEvaluationResultCacheDirty;

        bool Evaluate();

        /// <summary>
        /// 设置评估结果缓存过期。
        /// </summary>
        void SetEvaluationResultCacheDirty()
        {
            RaiseEvaluationResultCacheDirtyEvent();
        }

        /// <summary>
        /// 发起评估结果缓存过期事件。
        /// </summary>
        protected void RaiseEvaluationResultCacheDirtyEvent();
    }
}