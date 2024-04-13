using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.AI.Parameter
{
    /// <summary>
    /// 黑板数据。
    /// </summary>
    [Serializable]
    public class BlackboardData : IBlackboardData
    {
        /// <inheritdoc />
        IReadOnlyList<IParamData> IBlackboardData.Params => Params;

        /// <summary>
        /// 参数列表。
        /// </summary>
        public List<ParamData> Params = new List<ParamData>();
    }
}