using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ScenePartition
{
    [DisallowMultipleComponent]
    public partial class RootScene : MonoBehaviour, IValidatable
    {
        public ScenePartitionData PartitionData = new ScenePartitionData();


        public void Validate([NotNull] List<ValidationResult> results) { }
    }
}
