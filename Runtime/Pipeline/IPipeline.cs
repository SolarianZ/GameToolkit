using GBG.GameToolkit.Logic;
using System;

namespace GBG.GameToolkit.Process
{
    // Pipeline Stage Step

    public enum PipelineState
    {
        NotStarted,
        Running,
        Paused,
        Canceled,
        Completed,
    }

    public delegate void PipelineStateChangeHandler(IPipelineView pipeline, PipelineState newState, PipelineState oldState);

    public interface IPipelineView
    {
        int Id { get; }
        string Name { get; }
        int Priority { get; }
        bool KeepTickOnPause { get; set; }
        PipelineState State { get; }

        event PipelineStateChangeHandler StateChanged;
        event Action<IPipelineView> Stopped;

        /// <summary>
        /// Get the pipeline execution progress(in range [0,1]).
        /// </summary>
        /// <returns></returns>
        float GetProgress();
    }

    public interface IPipeline : IPipelineView, ITickable
    {
        void Start();
        void Pause();
        void Resume();
        void Cancel();
    }
}
