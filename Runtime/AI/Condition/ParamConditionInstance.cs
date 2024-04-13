using System;
using GBG.GameToolkit.AI.Common;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 黑板参数条件。
    /// </summary>
    public class ParamConditionInstance : ConditionInstanceBase
    {
        /// <inheritdoc/>
        public override bool UseEvaluationResultCache => true;
        internal ParamType ParamType;
        internal ConditionOperator Operator;
        internal IParamInstance LeftParam;
        internal IParamLiteral LeftParamLiteral;
        internal IParamInstance RightParam;
        internal IParamLiteral RightParamLiteral;

        private bool? _evaluationResultCache;


        public ParamConditionInstance(IBlackboardInstance blackboard, IParamConditionData data) : base(blackboard)
        {
            if (!ConditionHelper.CheckOperator(data.ParamType, data.Operator))
            {
                throw new ArgumentException($"Invalid operator({data.Operator}) for the given param type({data.ParamType}).");
            }

            ParamType = data.ParamType;
            Operator = data.Operator;

            if (data.LeftParamIsLiteral())
            {
                Debugger.Assert(ParamType == data.LeftParamLiteral.Type, "Left literal param type mismatch.");
                LeftParamLiteral = data.LeftParamLiteral;
            }
            else
            {
                LeftParam = blackboard.GetParamByGuidWithException(data.LeftParamGuid, ParamType);
                LeftParam.OnValueChanged += OnParamValueChanged;
            }

            if (data.RightParamIsLiteral())
            {
                Debugger.Assert(ParamType == data.RightParamLiteral.Type, "Right literal param type mismatch.");
                RightParamLiteral = data.RightParamLiteral;
            }
            else
            {
                RightParam = blackboard.GetParamByGuidWithException(data.RightParamGuid, ParamType);
                RightParam.OnValueChanged += OnParamValueChanged;
            }
        }

        private void OnParamValueChanged(IParamInstance _)
        {
            _evaluationResultCache = null;
            SetEvaluationResultCacheDirty();
        }

        /// <inheritdoc/>
        public override bool Evaluate()
        {
            if (!_evaluationResultCache.HasValue)
            {
                IParamValueProvider lhs = (IParamValueProvider)LeftParam ?? LeftParamLiteral;
                IParamValueProvider rhs = (IParamValueProvider)RightParam ?? RightParamLiteral;
                switch (Operator)
                {
                    case ConditionOperator.Equal:
                        _evaluationResultCache = ConditionHelper.ParamEquals(ParamType, lhs, rhs);
                        break;
                    case ConditionOperator.NotEqual:
                        _evaluationResultCache = ConditionHelper.ParamNotEquals(ParamType, lhs, rhs);
                        break;
                    case ConditionOperator.GreaterThan:
                        _evaluationResultCache = ConditionHelper.ParamGreaterThan(ParamType, lhs, rhs);
                        break;
                    case ConditionOperator.LessThan:
                        _evaluationResultCache = ConditionHelper.ParamLessThan(ParamType, lhs, rhs);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Operator), Operator, null);
                }
            }

            return _evaluationResultCache.Value;
        }
    }
}