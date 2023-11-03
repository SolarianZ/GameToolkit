using System.Collections.Generic;

namespace GBG.Framework.Process
{
    public interface IComplexPipelineView : IPipelineView, IEnumerable<IPipelineView>
    {
        int DirectSubPipelineCount { get; }
        IPipelineView this[int index] { get; }
    }

    public interface IComplexPipeline : IPipeline, IComplexPipelineView
    {
        bool ContainsSubPipeline(IPipeline pipeline);
        void AddSubPipeline(IPipeline pipeline);
        bool TryAddSubPipeline(IPipeline pipeline);
        bool RemoveSubPipeline(IPipeline pipeline);
        void ClearSubPipelines();
    }
}
