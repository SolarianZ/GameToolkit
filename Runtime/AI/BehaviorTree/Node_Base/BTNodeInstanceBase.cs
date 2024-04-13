using System.Collections.Generic;
using GBG.GameToolkit.AI.Condition;

namespace GBG.GameToolkit.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点。
    /// </summary>
    public abstract class BTNodeInstanceBase : IBTNodeInstance
    {
        /// <inheritdoc/>
        public BTContext Context { get; }
        /// <inheritdoc/>
        public string Guid { get; }
        /// <inheritdoc/>
        public string Name { get; }
        /// <inheritdoc/>
        IReadOnlyList<IConditionGroupInstance> IBTNodeInstance.PreconditionGroups => _preconditionGroups;
        private readonly IReadOnlyList<IConditionGroupInstance> _preconditionGroups;


        #region Connection

        /// <inheritdoc/>
        string IBTNodeInstance.ParentNodeGuid => _parentNodeGuid;
        private readonly string _parentNodeGuid;

        /// <inheritdoc/>
        IBTNodeInstance IBTNodeInstance.ParentNode
        {
            get => ParentNode;
            set => ParentNode = value;
        }
        protected IBTNodeInstance ParentNode { get; private set; }

        #endregion


        #region Lifecycle

        protected BTNodeInstanceBase(BTNodeConstructInfo constructInfo)
        {
            Context = constructInfo.Context;
            Guid = constructInfo.Guid;
            Name = constructInfo.Name;
            _preconditionGroups = constructInfo.PreconditionGroups;
            _parentNodeGuid = constructInfo.ParentGuid;
        }


        /// <inheritdoc/>
        void IBTNodeInstance.Initialize()
        {
            OnInitialize();
        }

        /// <summary>
        /// 初始化节点。
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <inheritdoc/>
        void IBTNodeInstance.Reset() { OnReset(); }

        /// <summary>
        /// 重置节点状态。
        /// </summary>
        protected virtual void OnReset() { }

        /// <inheritdoc/>
        void IBTNodeInstance.Abort()
        {
            if (!((IBTNodeInstance)this).IsInProgress)
            {
                return;
            }

            OnAborted();
            LastExecutionResult = BTNodeResult.Failure;
        }

        /// <summary>
        /// 中止执行节点。
        /// </summary>
        protected virtual void OnAborted() { }

        #endregion


        #region Execution

        /// <inheritdoc/>
        BTNodeResult IBTNodeInstance.LastExecutionResult
        {
            get => LastExecutionResult;
            set => LastExecutionResult = value;
        }
        protected BTNodeResult LastExecutionResult { get; private set; }

        /// <inheritdoc/>
        BTNodeResult IBTNodeInstance.ExecuteImpl()
        {
            return ExecuteImpl();
        }

        /// <summary>
        /// 执行节点。
        /// </summary>
        /// <returns>节点执行结果。</returns>
        protected abstract BTNodeResult ExecuteImpl();

        /// <inheritdoc/>
        IBTNodeInstance IBTNodeInstance.GetNextPassiveExecutionNode()
        {
            return GetNextPassiveExecutionNode();
        }

        /// <summary>
        /// 在被动执行模式下，获取下一个被动执行的节点。
        /// </summary>
        /// <returns>下一个被动执行的节点。</returns>
        /// <seealso cref="BehaviorTreeData.AlwaysExecuteFromRoot"/>
        /// <seealso cref="BTContext.ExecuteInPassiveMode"/>
        /// <seealso cref="BTContext.PassiveExecutionNode"/>
        internal abstract IBTNodeInstance GetNextPassiveExecutionNode();

        #endregion
    }
}