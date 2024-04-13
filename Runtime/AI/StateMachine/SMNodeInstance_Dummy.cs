namespace GBG.GameToolkit.AI.StateMachine
{
    /// <summary>
    /// 状态机节点 - 空状态。
    /// </summary>
    internal class SMNodeInstance_Dummy : SMNodeInstanceBase
    {
        public SMNodeInstance_Dummy(SMContext context, SMNodeInfo nodeInfo) : base(context, nodeInfo)
        {
        }

        /// <inheritdoc/>
        protected override void OnExecute() { }
    }
}
