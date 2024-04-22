using System.Runtime.CompilerServices;

namespace GBG.GameToolkit.Process
{
    public class SequenceComplexPipeline : ComplexPipelineBase
    {
        public int ActivePipelineIndex { get; private set; }
        private bool _skipLateTick;


        public SequenceComplexPipeline(int id, string name, int priority, int tickChannel,
            SubPipelineSortingMode subPipelineSortingMode = SubPipelineSortingMode.ByPriorityDesc, int capacity = 0)
            : base(id, name, priority, tickChannel, subPipelineSortingMode, capacity)
        {
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

            float progress = (ActivePipelineIndex + SubPipelineList[ActivePipelineIndex].GetProgress()) / DirectSubPipelineCount;
            return progress;
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
            if (ActivePipelineIndex >= DirectSubPipelineCount)
            {
                return;
            }

            IPipeline activePipeline = SubPipelineList[ActivePipelineIndex];
            if (isLateTick)
            {
                if (!_skipLateTick && PipelineCanTick(activePipeline))
                {
                    activePipeline.LateTick();
                }
            }
            else if (PipelineCanTick(activePipeline))
            {
                activePipeline.Tick();
            }
            _skipLateTick = false;

            if (activePipeline.IsStopped)
            {
                ActivePipelineIndex++;
                if (!isLateTick)
                {
                    _skipLateTick = true;
                }
            }
        }

        protected override void OnStart()
        {
            ActivePipelineIndex = 0;
            _skipLateTick = false;

            base.OnStart();
        }

        protected override void OnCancel()
        {
            base.OnCancel();

            ActivePipelineIndex = -1;
        }

        protected override void OnComplete()
        {
            ActivePipelineIndex = -1;
        }

        protected override bool IsExecutionComplete()
        {
            return ActivePipelineIndex >= DirectSubPipelineCount;
        }
    }
}
