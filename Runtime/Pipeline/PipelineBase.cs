using GBG.GameToolkit.Logic;
using System;

namespace GBG.GameToolkit.Process
{
    public abstract class PipelineBase : IPipeline
    {
        public int Id { get; }
        public string Name { get; }
        public int Priority { get; }
        public int TickChannel { get; }
        public int TickPriority => Priority;
        public bool KeepTickOnPause { get; set; }
        public PipelineState State { get; private set; }

        public event PipelineStateChangeHandler StateChanged;


        protected PipelineBase(int id, string name, int priority, int tickChannel)
        {
            Id = id;
            Name = name;
            Priority = priority;
            TickChannel = tickChannel;
        }

        void ITickable.Tick()
        {
            if (State == PipelineState.Running)
            {
                OnTick();
                if (GetProgress() >= 1)
                {
                    ChangeStateAndRaiseEvent(PipelineState.Completed);
                }
            }
            else if (KeepTickOnPause && State == PipelineState.Paused)
            {
                OnTick();
            }
        }

        public abstract void OnTick();

        void ITickable.LateTick()
        {
            if (State == PipelineState.Running)
            {
                OnLateTick();
                if (GetProgress() >= 1)
                {
                    ChangeStateAndRaiseEvent(PipelineState.Completed);
                }
            }
            else if (KeepTickOnPause && State == PipelineState.Paused)
            {
                OnLateTick();
            }
        }

        public abstract void OnLateTick();

        /// <inheritdoc/>
        public abstract float GetProgress();

        void IPipeline.Start()
        {
            if (State != PipelineState.NotStarted)
            {
                throw new InvalidOperationException($"Cannot start the pipeline from the '{State}' state.");
            }

            OnStart();
            ChangeStateAndRaiseEvent(PipelineState.Running);
        }

        protected abstract void OnStart();

        void IPipeline.Pause()
        {
            if (State != PipelineState.Running)
            {
                throw new InvalidOperationException($"Cannot pause the pipeline from the '{State}' state.");
            }

            OnPause();
            ChangeStateAndRaiseEvent(PipelineState.Paused);
        }

        protected abstract void OnPause();

        void IPipeline.Resume()
        {
            if (State != PipelineState.Paused)
            {
                throw new InvalidOperationException($"Cannot resume the pipeline from the '{State}' state.");
            }

            OnResume();
            ChangeStateAndRaiseEvent(PipelineState.Running);
        }

        protected abstract void OnResume();

        void IPipeline.Cancel()
        {
            if (State != PipelineState.Running && State != PipelineState.Paused)
            {
                throw new InvalidOperationException($"Cannot cancel the pipeline from the '{State}' state.");
            }

            OnCancel();
            ChangeStateAndRaiseEvent(PipelineState.Canceled);
        }

        protected abstract void OnCancel();


        private void ChangeStateAndRaiseEvent(PipelineState newState)
        {
            Debugger.Assert(State != newState,
                $"{nameof(PipelineBase)}.{nameof(State)} != {nameof(newState)}.");

            PipelineState oldState = State;
            State = newState;
            StateChanged?.Invoke(this, newState, oldState);
        }
    }
}
