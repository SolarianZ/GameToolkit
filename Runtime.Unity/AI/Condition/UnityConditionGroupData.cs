using System;
using System.Collections.Generic;
using GBG.GameToolkit.AI.Condition;
using UnityEngine;

namespace GBG.GameToolkit.Unity.AI.Condition
{
    /// <summary>
    /// 条件组数据。<br/>
    /// 条件组中的所有条件均通过检测时，条件组才能通过检测。
    /// </summary>
    [Serializable]
    public class UnityConditionGroupData : IConditionGroupData
    {
        /// <inheritdoc />
        IReadOnlyList<IConditionData> IConditionGroupData.Conditions => Conditions;

        /// <summary>
        /// 组内条件列表。
        /// </summary>
        [SerializeReference]
        public List<IConditionData> Conditions = new List<IConditionData>();
    }
}