namespace GBG.GameToolkit.Process
{
    public class SequenceComplexPipeline : ComplexPipelineBase
    {
        public int ActivePipelineIndex { get; private set; }
        private bool _skipLateTick;
        private float? _progressCache;


        public SequenceComplexPipeline(int id, string name, int priority, int tickChannel,
            SubPipelineSortingMode subPipelineSortingMode = SubPipelineSortingMode.ByPriorityDesc, int capacity = 0)
            : base(id, name, priority, tickChannel, subPipelineSortingMode, capacity)
        {
        }

        public override float GetProgress()
        {
            if (_progressCache.HasValue)
            {
                return _progressCache.Value;
            }

            float denominator = 0;
            float numerator = 0;
            for (int i = 0; i < DirectSubPipelineCount; i++)
            {
                IPipeline pipeline = SubPipelineList[i];
                numerator += pipeline.GetProgress();
                denominator++;
            }

            if (denominator == 0)
            {
                return 1;
            }

            float progress = numerator / denominator;
            return progress;
        }

        public override void OnTick()
        {
            EvaluateActivePipeline(false);
        }

        public override void OnLateTick()
        {
            EvaluateActivePipeline(true);
        }

        private void EvaluateActivePipeline(bool isLateTick)
        {
            if (ActivePipelineIndex >= DirectSubPipelineCount)
            {
                return;
            }

            IPipeline activePipeline = SubPipelineList[ActivePipelineIndex];
            if (isLateTick)
            {
                if (!_skipLateTick)
                {
                    activePipeline.LateTick();
                }
                _skipLateTick = false;
            }
            else
            {
                activePipeline.Tick();
            }

            if (activePipeline.State == PipelineState.Completed ||
                activePipeline.State == PipelineState.Canceled)
            {
                ActivePipelineIndex++;
                if (!isLateTick)
                {
                    _skipLateTick = true;
                }
            }

            _progressCache = null;
        }

        protected override void OnStart()
        {
            ActivePipelineIndex = 0;
            _skipLateTick = false;
            _progressCache = null;

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
    }
}
