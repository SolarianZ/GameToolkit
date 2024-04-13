using GBG.GameToolkit.AI.Parameter;

namespace GBG.GameToolkit.AI.StateMachine
{
    public static class StateMachineHelper
    {
        public static SMNodeInfo CreateNodeInfo(this ISMNodeData nodeData, IBlackboardInstance blackboard)
        {
            ISMTransitionInstance[] transitions = nodeData.CreateTransitionInstances(blackboard);
            SMNodeInfo nodeInfo = new SMNodeInfo(nodeData, blackboard, transitions);
            return nodeInfo;
        }

        public static ISMTransitionInstance[] CreateTransitionInstances(this ISMNodeData nodeData, IBlackboardInstance blackboard)
        {
            if (nodeData.Transitions == null)
            {
                return null;
            }

            ISMTransitionInstance[] conditionGroupInstances = new ISMTransitionInstance[nodeData.Transitions.Count];
            for (int i = 0; i < nodeData.Transitions.Count; i++)
            {
                ISMTransitionData conditionGroupData = nodeData.Transitions[i];
                ISMTransitionInstance conditionGroupInstance = new SMTransitionInstance(blackboard, conditionGroupData);
                conditionGroupInstances[i] = conditionGroupInstance;
            }

            return conditionGroupInstances;
        }
    }
}