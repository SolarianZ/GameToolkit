namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件组。<br/>
    /// 条件组中的所有条件均通过检测时，条件组才能通过检测。
    /// </summary>
    public interface IConditionGroupInstance
    {
        bool UseEvaluationResultCache { get; }
        
        /// <summary>
        /// 评估条件组。<br/>
        /// 条件组中的所有条件均通过检测时，条件组才能通过检测。
        /// </summary>
        /// <returns>条件组是否通过检测。</returns>
        bool Evaluate();
    }
}