using System;

namespace GBG.GameToolkit.Process
{
    public class SequenceComplexPipeline : ComplexPipelineBase
    {
        private float? _progressCache;


        public SequenceComplexPipeline(int id, string name, int priority, int tickChannel, int capacity = 0)
            : base(id, name, priority, tickChannel, capacity)
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
                return 0;
            }

            float progress = numerator / denominator;
            return progress;
        }

        public override void OnTick()
        {
            _progressCache = null;
            throw new NotImplementedException();
        }

        public override void OnLateTick()
        {
            _progressCache = null;
            throw new NotImplementedException();
        }
    }
}
