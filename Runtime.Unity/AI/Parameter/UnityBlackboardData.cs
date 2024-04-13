using System;
using System.Collections.Generic;
using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.Unity.Parameter
{
    /// <summary>
    /// 黑板数据。
    /// </summary>
    [Serializable]
    public class UnityBlackboardData : IBlackboardData
    {
        /// <inheritdoc />
        IReadOnlyList<IParamData> IBlackboardData.Params => Params;

        /// <summary>
        /// 参数列表。
        /// </summary>
        public List<UnityParamData> Params = new List<UnityParamData>();
    }
}