using System.Collections.Generic;
using GBG.GameToolkit.AI.Condition;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点。
    /// </summary>
    public interface IBTNodeInstance
    {
        /// <summary>
        /// 行为树上下文。
        /// </summary>
        BTContext Context { get; }
        /// <summary>
        /// 节点GUID。
        /// </summary>
        string Guid { get; }
        /// <summary>
        /// 节点名称。
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 节点可执行的前置条件。
        /// </summary>
        IReadOnlyList<IConditionGroupInstance> PreconditionGroups { get; }


        #region Connection

        /// <summary>
        /// 父节点GUID。
        /// </summary>
        internal string ParentNodeGuid { get; }
        /// <summary>
        /// 父节点实例。
        /// </summary>
        internal IBTNodeInstance ParentNode { get; set; }

        #endregion


        #region Lifecycle

        /// <summary>
        /// 初始化节点。
        /// </summary>
        internal void Initialize();

        /// <summary>
        /// 重置节点状态。
        /// </summary>
        internal void Reset();

        /// <summary>
        /// 中止执行节点。
        /// </summary>
        internal void Abort();

        #endregion


        #region Execution

        /// <summary>
        /// 节点上次执行结果。
        /// </summary>
        BTNodeResult LastExecutionResult { get; internal set; }
        /// <summary>
        /// 节点是否尚未执行结束。
        /// </summary>
        // 不要对外公开，因为此属性值只在特定的时机才有意义
        internal bool IsInProgress => LastExecutionResult == BTNodeResult.InProgress;


        /// <summary>
        /// 执行节点。
        /// </summary>
        /// <returns>节点执行结果。</returns>
        internal BTNodeResult Execute()
        {
            if (EvaluatePreconditions())
            {
                LastExecutionResult = ExecuteImpl();
            }
            else if (IsInProgress)
            {
                Abort();
            }
            else
            {
                LastExecutionResult = BTNodeResult.Failure;
            }

            return LastExecutionResult;
        }

        /// <summary>
        /// 执行节点。
        /// </summary>
        /// <returns>节点执行结果。</returns>
        protected BTNodeResult ExecuteImpl();

        /// <summary>
        /// 计算是否满足节点可执行的前置条件。
        /// </summary>
        /// <returns>true表示满足，false表示不满足。</returns>
        internal bool EvaluatePreconditions()
        {
            if (PreconditionGroups == null)
            {
                return true;
            }

            for (int i = 0; i < PreconditionGroups.Count; i++)
            {
                IConditionGroupInstance group = PreconditionGroups[i];
                if (group.Evaluate())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 在被动执行模式下，获取下一个被动执行的节点。
        /// </summary>
        /// <returns>下一个被动执行的节点。</returns>
        /// <seealso cref="BehaviorTreeData.AlwaysExecuteFromRoot"/>
        /// <seealso cref="BTContext.ExecuteInPassiveMode"/>
        /// <seealso cref="BTContext.PassiveExecutionNode"/>
        internal IBTNodeInstance GetNextPassiveExecutionNode();

        #endregion
    }
}