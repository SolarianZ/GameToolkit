using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.AI.Condition
{
    /// <summary>
    /// 条件组数据。<br/>
    /// 条件组中的所有条件均通过检测时，条件组才能通过检测。
    /// </summary>
    [Serializable]
    public class ConditionGroupData : IConditionGroupData
    {
        /// <inheritdoc />
        IReadOnlyList<IConditionData> IConditionGroupData.Conditions => Conditions;

        /// <summary>
        /// 组内条件列表。
        /// </summary>
        public List<ConditionDataBase> Conditions = new List<ConditionDataBase>();
    }
}