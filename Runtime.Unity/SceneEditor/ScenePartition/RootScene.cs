using UnityEngine;

namespace GBG.GameToolkit.Unity.ScenePartition
{
    [DisallowMultipleComponent]
    public partial class RootScene : MonoBehaviour
    {
        public ScenePartitionData PartitionData = new ScenePartitionData();
    }
}
