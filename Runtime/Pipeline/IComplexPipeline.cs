using System.Collections.Generic;

namespace GBG.GameToolkit.Process
{
    public delegate void SubPipelineStageChangeHandler(IPipelineView rootPipeline,
        IPipelineView changedPipeline, int changedPipelineDepth,
        PipelineState newState, PipelineState oldState);

    public interface IComplexPipelineView : IPipelineView
    {
        int DirectSubPipelineCount { get; }
        IPipelineView this[int index] { get; }

        event SubPipelineStageChangeHandler SubPipelineStageChanged;
    }

    public interface IComplexPipeline : IPipeline, IComplexPipelineView
    {
        bool ContainsSubPipeline(IPipeline pipeline);
        bool ContainsSubPipeline(int pipelineId);
        void AddSubPipeline(IPipeline pipeline);
        bool TryAddSubPipeline(IPipeline pipeline);
        bool RemoveSubPipeline(IPipeline pipeline, bool cancelExecution);
        bool RemoveSubPipeline(int pipelineId, bool cancelExecution);
        void ClearSubPipelines(bool cancelExecution);
    }
}
