using GBG.GameToolkit.AI.BehaviorTree;
using GBG.GameToolkit.AI.Parameter;
using UnityEngine;

namespace GBG.GameToolkit.AI.Unity.BehaviorTree
{
    public class UnityBTTaskNodeInstance_SetActive : BTTaskNodeInstanceBase
    {
        private readonly IParamInstance _targetObjectParam;
        private readonly IParamInstance _activeStateParam;


        public UnityBTTaskNodeInstance_SetActive(BTNodeConstructInfo constructInfo,
            string targetObjectParamGuid, string activeStateParamGuid) : base(constructInfo)
        {
            _targetObjectParam = Context.Blackboard.GetParamByGuid(targetObjectParamGuid);
            if (_targetObjectParam == null)
            {
                Debug.LogError($"{nameof(UnityBTTaskNodeInstance_SetActive)}: Target object param guid is not valid: {targetObjectParamGuid}.");
            }
            else if (!_targetObjectParam.Type.IsObjectType())
            {
                Debug.LogError($"{nameof(UnityBTTaskNodeInstance_SetActive)}: Target object param type is not a valid: Expected object type, but got {_targetObjectParam.Type}.");
                _targetObjectParam = null;
            }

            _activeStateParam = Context.Blackboard.GetParamByGuid(activeStateParamGuid);
            if (_activeStateParam == null)
            {
                Debug.LogError($"{nameof(UnityBTTaskNodeInstance_SetActive)}: Active state param guid is not valid: {activeStateParamGuid}.");
            }
            else if (_activeStateParam.Type != ParamType.Bool)
            {
                Debug.LogError($"{nameof(UnityBTTaskNodeInstance_SetActive)}: Active state param type is not a valid: Expected bool type, but got {_activeStateParam.Type}.");
                _activeStateParam = null;
            }
        }

        protected override BTNodeResult ExecuteTask()
        {
            if (_targetObjectParam == null || _activeStateParam == null)
            {
                return BTNodeResult.Failure;
            }

            if (!_activeStateParam.TryGetObject(out bool activeState))
            {
                return BTNodeResult.Failure;
            }

            if (!_targetObjectParam.TryGetObject(out Object targetObject))
            {
                return BTNodeResult.Failure;
            }

            if (targetObject is GameObject gameObject)
            {
                if (gameObject.activeSelf != activeState)
                {
                    gameObject.SetActive(activeState);
                }

                return BTNodeResult.Success;
            }

            if (targetObject is Component component)
            {
                gameObject = component.gameObject;
                if (gameObject.activeSelf != activeState)
                {
                    gameObject.SetActive(activeState);
                }

                return BTNodeResult.Success;
            }

            return BTNodeResult.Failure;
        }
    }
}