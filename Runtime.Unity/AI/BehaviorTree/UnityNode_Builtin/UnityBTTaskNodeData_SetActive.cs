using System;
using GBG.GameToolkit.AI.BehaviorTree;

namespace GBG.GameToolkit.Unity.AI.BehaviorTree
{
    [Serializable]
    public class UnityBTTaskNodeData_SetActive : UnityBTTaskNodeDataBase
    {
        public string TargetObjectParamGuid;
        public string ActiveStateParamGuid;


        public override IBTNodeInstance CreateNodeInstance(BTContext context)
        {
            BTNodeConstructInfo constructInfo = this.CreateNodeConstructInfo(context);
            UnityBTTaskNodeInstance_SetActive instance = new UnityBTTaskNodeInstance_SetActive(constructInfo,
                TargetObjectParamGuid, ActiveStateParamGuid);
            return instance;
        }
    }
}