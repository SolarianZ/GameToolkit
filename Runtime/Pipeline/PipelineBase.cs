using GBG.GameToolkit.Logic;
using System;
using System.Runtime.CompilerServices;

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
        public bool IsStopped => State == PipelineState.Completed || State == PipelineState.Canceled;

        public event PipelineStateChangeHandler StateChanged;
        public event Action<IPipelineView> Stopped;


        protected PipelineBase(int id, string name, int priority, int tickChannel)
        {
            Id = id;
            Name = name;
            Priority = priority;
            TickChannel = tickChannel;
        }

        public abstract float GetProgress();

        void ITickable.Tick()
        {
            Evaluate(false);
        }

        void ITickable.LateTick()
        {
            Evaluate(true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Evaluate(bool isLateTick)
        {
            switch (State)
            {
                case PipelineState.Running:
                    if (isLateTick) { OnLateTick(); }
                    else { OnTick(); }

                    if (IsExecutionComplete())
                    {
                        OnComplete();
                        ChangeStateAndRaiseEvent(PipelineState.Completed);
                    }
                    break;
                case PipelineState.Paused:
                    if (KeepTickOnPause)
                    {
                        if (isLateTick) { OnLateTick(); }
                        else { OnTick(); }
                    }
                    break;

                case PipelineState.NotStarted:
                case PipelineState.Canceled:
                case PipelineState.Completed:
                    throw new InvalidOperationException($"Cannot {(isLateTick ? "late tick" : "tick")} the pipeline while in state '{State}'.");

                default:
                    throw new ArgumentOutOfRangeException(nameof(State), State, null);
            }
        }

        public abstract void OnTick();
        public abstract void OnLateTick();

        void IPipeline.Start()
        {
            if (State != PipelineState.NotStarted)
            {
                throw new InvalidOperationException($"Cannot start the pipeline while in state '{State}'.");
            }

            OnStart();
            ChangeStateAndRaiseEvent(PipelineState.Running);
        }

        protected abstract void OnStart();

        void IPipeline.Pause()
        {
            if (State != PipelineState.Running)
            {
                throw new InvalidOperationException($"Cannot pause the pipeline while in state '{State}'.");
            }

            OnPause();
            ChangeStateAndRaiseEvent(PipelineState.Paused);
        }

        protected abstract void OnPause();

        void IPipeline.Resume()
        {
            if (State != PipelineState.Paused)
            {
                throw new InvalidOperationException($"Cannot resume the pipeline while in state '{State}'.");
            }

            OnResume();
            ChangeStateAndRaiseEvent(PipelineState.Running);
        }

        protected abstract void OnResume();

        void IPipeline.Cancel()
        {
            if (State != PipelineState.Running && State != PipelineState.Paused)
            {
                throw new InvalidOperationException($"Cannot cancel the pipeline while in state '{State}'.");
            }

            OnCancel();
            ChangeStateAndRaiseEvent(PipelineState.Canceled);
        }

        protected abstract void OnCancel();

        protected abstract void OnComplete();

        protected abstract bool IsExecutionComplete();


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ChangeStateAndRaiseEvent(PipelineState newState)
        {
            Debugger.Assert(State != newState,
                $"{nameof(PipelineBase)}.{nameof(State)} != {nameof(newState)}.");

            PipelineState oldState = State;
            State = newState;
            StateChanged?.Invoke(this, newState, oldState);
            if (newState == PipelineState.Completed ||
                newState == PipelineState.Canceled)
            {
                Stopped?.Invoke(this);
            }
        }
    }
}
