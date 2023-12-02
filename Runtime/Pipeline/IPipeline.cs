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

    public interface IPipelineView
    {
        int Id { get; }
        string Name { get; }
        int Priority { get; }
        float Progress { get; }
        //int Stage { get; }
        PipelineState State { get; }

        event Action<IPipelineView> Started;
        event Action<IPipelineView> Paused;
        event Action<IPipelineView> Resumed;
        event Action<IPipelineView> Canceled;
        event Action<IPipelineView> Completed;
        event Action<IPipelineView, PipelineState, PipelineState> StateChanged;
    }

    public interface IPipeline : IPipelineView
    {
        void Start();
        void Pause();
        void Resume();
        void Cancel();
    }
}
