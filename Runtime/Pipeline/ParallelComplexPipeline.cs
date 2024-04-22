using System;
using System.Runtime.CompilerServices;

namespace GBG.GameToolkit.Process
{
    public class ParallelComplexPipeline : ComplexPipelineBase
    {
        public enum PipelineCompletionMode
        {
            AllSubPipelinesStopped,
            AnySubPipelineCompleted,
            AnySubPipelineStopped,
            FirstPipelineStopped,
        }


        public PipelineCompletionMode CompletionMode { get; private set; }
        private float? _progressCache;


        public ParallelComplexPipeline(int id, string name, int priority, int tickChannel,
            PipelineCompletionMode completionMode = PipelineCompletionMode.AllSubPipelinesStopped,
            SubPipelineSortingMode subPipelineSortingMode = SubPipelineSortingMode.ByPriorityDesc, int capacity = 0)
            : base(id, name, priority, tickChannel, subPipelineSortingMode, capacity)
        {
            CompletionMode = completionMode;
        }

        public void SetCompletionMode(PipelineCompletionMode completionMode)
        {
            if (State != PipelineState.NotStarted &&
                State != PipelineState.Completed &&
                State != PipelineState.Canceled)
            {
                throw new InvalidOperationException($"Cannot set the completion mode while in state '{State}'.");
            }

            CompletionMode = completionMode;
        }

        public override float GetProgress()
        {
            if (State == PipelineState.NotStarted)
            {
                return 0;
            }

            if (IsStopped)
            {
                return 1;
            }

            if (_progressCache.HasValue)
            {
                return _progressCache.Value;
            }

            switch (CompletionMode)
            {
                case PipelineCompletionMode.AllSubPipelinesStopped:
                {
                    _progressCache = GetAccumulativeSubPipelineProgress();
                    break;
                }
                case PipelineCompletionMode.AnySubPipelineCompleted:
                case PipelineCompletionMode.AnySubPipelineStopped:
                {
                    _progressCache = GetMaxSubPipelineProgress();
                    break;
                }
                case PipelineCompletionMode.FirstPipelineStopped:
                {
                    _progressCache = SubPipelineList[0].GetProgress();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(CompletionMode), CompletionMode, null);
            }

            return _progressCache.Value;
        }

        private float GetAccumulativeSubPipelineProgress()
        {
            float denominator = 0;
            float numerator = 0;
            for (int i = 0; i < DirectSubPipelineCount; i++)
            {
                IPipeline pipeline = SubPipelineList[i];
                if (pipeline.IsStopped)
                {
                    numerator += 1;
                }
                else
                {
                    numerator += pipeline.GetProgress();
                }
                denominator++;
            }

            if (denominator == 0)
            {
                return 1;
            }

            float progress = numerator / denominator;
            return progress;
        }

        private float GetMaxSubPipelineProgress()
        {
            float maxProgress = 0;
            for (int i = 0; i < DirectSubPipelineCount; i++)
            {
                IPipeline pipeline = SubPipelineList[i];
                float progress = pipeline.GetProgress();
                if (progress > maxProgress)
                {
                    maxProgress = progress;
                }
            }

            return maxProgress;
        }

        public override void OnTick()
        {
            Evaluate(false);
        }

        public override void OnLateTick()
        {
            Evaluate(true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Evaluate(bool isLateTick)
        {
            bool allStopped = true;
            for (int i = 0; i < DirectSubPipelineCount; i++)
            {
                IPipeline pipeline = SubPipelineList[i];
                if (PipelineCanTick(pipeline))
                {
                    if (isLateTick) { pipeline.LateTick(); }
                    else { pipeline.Tick(); }
                }

                switch (CompletionMode)
                {
                    case PipelineCompletionMode.AllSubPipelinesStopped:
                        allStopped &= pipeline.IsStopped;
                        if (i == DirectSubPipelineCount - 1)
                        {
                            _isCompleted = allStopped;
                        }
                        break;
                    case PipelineCompletionMode.AnySubPipelineCompleted:
                        _isCompleted = pipeline.State == PipelineState.Completed;
                        break;
                    case PipelineCompletionMode.AnySubPipelineStopped:
                        _isCompleted = pipeline.IsStopped;
                        break;
                    case PipelineCompletionMode.FirstPipelineStopped:
                        if (i == 0)
                        {
                            _isCompleted = pipeline.IsStopped;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(CompletionMode), CompletionMode, null);
                }

                if (_isCompleted)
                {
                    break;
                }
            }

            _progressCache = null;
        }

        protected override void OnStart()
        {
            base.OnStart();
            _isCompleted = false;
        }

        protected override void OnComplete()
        {
            for (int i = 0; i < DirectSubPipelineCount; i++)
            {
                IPipeline pipeline = SubPipelineList[i];
                if (pipeline.State == PipelineState.Running ||
                    pipeline.State == PipelineState.Paused)
                {
                    pipeline.Cancel();
                }
            }
        }

        protected override bool IsExecutionComplete()
        {
            if (DirectSubPipelineCount == 0)
            {
                return true;
            }

            return _isCompleted;
        }


        private bool _isCompleted;
    }
}
