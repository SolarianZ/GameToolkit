using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件组。<br/>
    /// 条件组中的所有条件均通过检测时，条件组才能通过检测。
    /// </summary>
    public class ConditionGroupInstance : IConditionGroupInstance
    {
        public bool UseEvaluationResultCache { get; }
        private readonly IConditionInstance[] _conditionInstances;
        private bool? _evaluationResultCache;


        public ConditionGroupInstance(IBlackboardInstance blackboard, IConditionGroupData data)
        {
            UseEvaluationResultCache = true;
            if (data.Conditions != null && data.Conditions.Count > 0)
            {
                _conditionInstances = new IConditionInstance[data.Conditions.Count];
                for (int i = 0; i < data.Conditions.Count; i++)
                {
                    IConditionData conditionData = data.Conditions[i];
                    IConditionInstance conditionInstance = conditionData.CreateConditionInstance(blackboard);
                    if (UseEvaluationResultCache && conditionInstance.UseEvaluationResultCache)
                    {
                        conditionInstance.OnEvaluationResultCacheDirty += OnConditionEvaluationResultCacheDirty;
                    }
                    else
                    {
                        UseEvaluationResultCache = false;
                    }

                    _conditionInstances[i] = conditionInstance;
                }
            }
        }

        private void OnConditionEvaluationResultCacheDirty()
        {
            _evaluationResultCache = null;
        }

        /// <inheritdoc/>
        public bool Evaluate()
        {
            if (UseEvaluationResultCache && _evaluationResultCache.HasValue)
            {
                return _evaluationResultCache.Value;
            }

            if (_conditionInstances != null)
            {
                foreach (var condition in _conditionInstances)
                {
                    if (!condition.Evaluate())
                    {
                        _evaluationResultCache = false;
                        return false;
                    }
                }
            }

            _evaluationResultCache = true;
            return true;
        }
    }
}