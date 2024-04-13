namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机节点。
    /// </summary>
    public abstract class SMNodeInstanceBase : ISMNodeInstance
    {
        /// <summary>
        /// 状态机上下文。
        /// </summary>
        public SMContext Context { get; }
        /// <summary>
        /// 节点信息。
        /// </summary>
        public SMNodeInfo Info { get; }


        #region Lifecycle

        protected SMNodeInstanceBase(SMContext context, SMNodeInfo nodeInfo)
        {
            Context = context;
            Info = nodeInfo;
        }

        /// <inheritdoc />
        void ISMNodeInstance.Initialize() { OnInitialize(); }

        /// <summary>
        /// 初始化节点。
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <inheritdoc />
        void ISMNodeInstance.Reset() { OnReset(); }

        /// <summary>
        /// 重置节点状态。
        /// </summary>
        protected virtual void OnReset() { }

        /// <inheritdoc />
        void ISMNodeInstance.Enter() { OnEnter(); }

        /// <summary>
        /// 进入节点。
        /// </summary>
        protected virtual void OnEnter() { }

        /// <inheritdoc />
        void ISMNodeInstance.Execute() { OnExecute(); }

        /// <summary>
        /// 执行节点。
        /// </summary>
        protected abstract void OnExecute();

        /// <inheritdoc />
        void ISMNodeInstance.Exit() { OnExit(); }

        /// <summary>
        /// 退出节点。
        /// </summary>
        protected virtual void OnExit() { }

        #endregion
    }
}