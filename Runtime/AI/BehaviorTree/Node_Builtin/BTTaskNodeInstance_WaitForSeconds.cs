namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树任务节点 - 等待指定的时间。
    /// </summary>
    public class BTTaskNodeInstance_WaitForSeconds : BTTaskNodeInstance_WaitForSecondsBase
    {
        /// <summary>
        /// 需要等待的时间（秒）。
        /// </summary>
        private float _waitTime;


        public BTTaskNodeInstance_WaitForSeconds(BTNodeConstructInfo constructInfo, float waitTime) : base(constructInfo)
        {
            _waitTime = waitTime;
        }

        /// <inheritdoc/>
        public override float GetWaitTime()
        {
            return _waitTime;
        }
    }
}
