namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机节点。
    /// </summary>
    public interface ISMNodeInstance
    {
        /// <summary>
        /// 状态机上下文。
        /// </summary>
        SMContext Context { get; }
        /// <summary>
        /// 节点信息。
        /// </summary>
        SMNodeInfo Info { get; }


        /// <summary>
        /// 初始化节点。
        /// </summary>
        internal void Initialize();

        /// <summary>
        /// 重置节点状态。
        /// </summary>
        internal void Reset();

        /// <summary>
        /// 进入节点。
        /// </summary>
        internal void Enter();

        /// <summary>
        /// 执行节点。
        /// </summary>
        internal void Execute();

        /// <summary>
        /// 退出节点。
        /// </summary>
        internal void Exit();

        /// <summary>
        /// 获取下一个可执行节点的GUID。
        /// </summary>
        /// <returns></returns>
        string FindNextExecutableNodeGuid()
        {
            if (Info.Transitions == null)
            {
                return null;
            }

            for (int i = 0; i < Info.Transitions.Count; i++)
            {
                if (Info.Transitions[i].Evaluate())
                {
                    return Info.Transitions[i].DestNodeGuid;
                }
            }

            return null;
        }
    }
}