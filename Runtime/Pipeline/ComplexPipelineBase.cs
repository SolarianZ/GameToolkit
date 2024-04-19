using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.Process
{
    public abstract class ComplexPipelineBase : PipelineBase, IComplexPipeline
    {
        public int DirectSubPipelineCount => _subPipelineList.Count;
        public IPipelineView this[int index] => _subPipelineList[index];
        protected IReadOnlyList<IPipeline> SubPipelineList => _subPipelineList;
        private readonly List<IPipeline> _subPipelineList;
        private readonly Dictionary<int, IPipeline> _subPipelineTable;

        public event SubPipelineStageChangeHandler SubPipelineStageChanged;


        protected ComplexPipelineBase(int id, string name, int priority, int tickChannel, int capacity = 0)
            : base(id, name, priority, tickChannel)
        {
            _subPipelineList = new List<IPipeline>(capacity);
            _subPipelineTable = new Dictionary<int, IPipeline>(capacity);
        }

        protected override void OnStart()
        {
            for (int i = 0; i < _subPipelineList.Count; i++)
            {
                IPipeline pipeline = _subPipelineList[i];
                pipeline.Start();
            }
        }

        protected override void OnPause()
        {
            for (int i = 0; i < _subPipelineList.Count; i++)
            {
                IPipeline pipeline = _subPipelineList[i];
                pipeline.Pause();
            }
        }

        protected override void OnResume()
        {
            for (int i = 0; i < _subPipelineList.Count; i++)
            {
                IPipeline pipeline = _subPipelineList[i];
                pipeline.Resume();
            }
        }

        protected override void OnCancel()
        {
            for (int i = 0; i < _subPipelineList.Count; i++)
            {
                IPipeline pipeline = _subPipelineList[i];
                pipeline.Cancel();
            }
        }


        public bool ContainsSubPipeline(IPipeline pipeline)
        {
            if (pipeline == null)
            {
                return false;
            }

            return ContainsSubPipeline(pipeline.Id);
        }

        public bool ContainsSubPipeline(int pipelineId)
        {
            return _subPipelineTable.ContainsKey(pipelineId);
        }

        public void AddSubPipeline(IPipeline pipeline)
        {
            if (State != PipelineState.NotStarted)
            {
                throw new InvalidOperationException("Sub-pipelines can only be added before the pipeline starts executing.");
            }

            if (pipeline == null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }

            _subPipelineTable.Add(pipeline.Id, pipeline);
            InsertSubPipelineToListByPriority(pipeline);

            pipeline.StateChanged += OnDirectSubPipelineStateChanged;
            if (pipeline is IComplexPipeline complexPipeline)
            {
                complexPipeline.SubPipelineStageChanged += OnComplexSubPipelineStageChanged;
            }
        }

        public bool TryAddSubPipeline(IPipeline pipeline)
        {
            if (State != PipelineState.NotStarted)
            {
                return false;
            }

            if (pipeline == null)
            {
                return false;
            }

            if (!_subPipelineTable.TryAdd(pipeline.Id, pipeline))
            {
                return false;
            }

            InsertSubPipelineToListByPriority(pipeline);

            pipeline.StateChanged += OnDirectSubPipelineStateChanged;
            if (pipeline is IComplexPipeline complexPipeline)
            {
                complexPipeline.SubPipelineStageChanged += OnComplexSubPipelineStageChanged;
            }

            return true;
        }

        private void InsertSubPipelineToListByPriority(IPipeline pipeline)
        {
            for (int i = _subPipelineList.Count - 1; i >= 0; i--)
            {
                IPipeline p = _subPipelineList[i];
                if (p.Priority <= pipeline.Priority)
                {
                    _subPipelineList.Insert(i + 1, pipeline);
                    return;
                }
            }

            _subPipelineList.Add(pipeline);
        }

        public bool RemoveSubPipeline(IPipeline pipeline, bool cancelExecution)
        {
            if (pipeline == null)
            {
                return false;
            }

            return RemoveSubPipeline(pipeline.Id, cancelExecution);
        }

        public bool RemoveSubPipeline(int pipelineId, bool cancelExecution)
        {
            if (_subPipelineTable.Remove(pipelineId, out IPipeline pipeline))
            {
                bool success = _subPipelineList.Remove(pipeline);
                Debugger.Assert(success, "_subPipelineList.Remove(pipeline) == true");

                pipeline.StateChanged -= OnDirectSubPipelineStateChanged;
                if (pipeline is IComplexPipeline complexPipeline)
                {
                    complexPipeline.SubPipelineStageChanged -= OnComplexSubPipelineStageChanged;
                }

                if (cancelExecution &&
                    pipeline.State == PipelineState.Running ||
                    pipeline.State == PipelineState.Paused)
                {
                    pipeline.Cancel();
                }

                return true;
            }

            return false;
        }

        public void ClearSubPipelines(bool cancelExecution)
        {
            for (int i = 0; i < _subPipelineList.Count; i++)
            {
                IPipeline pipeline = _subPipelineList[i];
                pipeline.StateChanged -= OnDirectSubPipelineStateChanged;
                if (pipeline is IComplexPipeline complexPipeline)
                {
                    complexPipeline.SubPipelineStageChanged -= OnComplexSubPipelineStageChanged;
                }

                if (cancelExecution &&
                    pipeline.State == PipelineState.Running ||
                    pipeline.State == PipelineState.Paused)
                {
                    pipeline.Cancel();
                }
            }

            _subPipelineList.Clear();
            _subPipelineTable.Clear();
        }


        private void OnDirectSubPipelineStateChanged(IPipelineView pipeline, PipelineState newState, PipelineState oldState)
        {
            SubPipelineStageChanged?.Invoke(this, pipeline, 1, newState, oldState);
        }

        private void OnComplexSubPipelineStageChanged(IPipelineView rootPipeline,
            IPipelineView changedPipeline, int changedPipelineDepth,
            PipelineState newState, PipelineState oldState)
        {
            SubPipelineStageChanged?.Invoke(this, changedPipeline, changedPipelineDepth + 1, newState, oldState);
        }
    }
}
