using System.Collections.Generic;

namespace GBG.GameToolkit.AI.Parameter
{
    /// <summary>
    /// 黑板数据。
    /// </summary>
    public interface IBlackboardData
    {
        /// <summary>
        /// 参数列表。
        /// </summary>
        IReadOnlyList<IParamData> Params { get; }
    }
}