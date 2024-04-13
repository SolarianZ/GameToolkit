using System.Collections.Generic;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件组数据。<br/>
    /// 条件组中的所有条件均通过检测时，条件组才能通过检测。
    /// </summary>
    public interface IConditionGroupData
    {
        IReadOnlyList<IConditionData> Conditions { get; }
    }
}