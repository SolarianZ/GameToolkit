#if UNITY_EDITOR
using System;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ScenePartition
{
    partial class RootScene
    {
        private void OnDrawGizmos()
        {
            EditorDrawPartitionGrid();
        }

        private void EditorDrawPartitionGrid()
        {
            Gizmos.color = _editorDebugSettings.PartitionGridColor;
            Gizmos.DrawSphere(PartitionData.SceneOrigin, 0.1f);

            float lineLen0 = PartitionData.PartitionLength0 * PartitionData.PartitionCount0;
            float lineLen1 = PartitionData.PartitionLength1 * PartitionData.PartitionCount1;
            if (lineLen0 == 0 || lineLen1 == 0)
            {
                return;
            }

            Vector3 forward0, forward1;
            switch (PartitionData.PartitionType)
            {
                case ScenePartitionType.XZ:
                    forward0 = Vector3.right;
                    forward1 = Vector3.forward;
                    break;

                case ScenePartitionType.XY:
                    forward0 = Vector3.right;
                    forward1 = Vector3.up;
                    break;

                default:
                    throw new Exception($"Unknown scene partition type: {PartitionData.PartitionType}.");
            }

            // Lines 0
            for (int i = 0; i <= PartitionData.PartitionCount0; i++)
            {
                var lineStart = PartitionData.PartitionLength0 * i * forward0 + PartitionData.SceneOrigin;
                var lineEnd = lineLen1 * forward1 + lineStart;
                Gizmos.DrawLine(lineStart, lineEnd);
            }

            // Lines 1
            for (int i = 0; i <= PartitionData.PartitionCount1; i++)
            {
                var lineStart = PartitionData.PartitionLength1 * i * forward1 + PartitionData.SceneOrigin;
                var lineEnd = lineLen0 * forward0 + lineStart;
                Gizmos.DrawLine(lineStart, lineEnd);
            }
        }


        [Header("Debug Settings")]
        [SerializeField]
        private EditorDebugSettings _editorDebugSettings = new EditorDebugSettings();

        [Serializable]
        class EditorDebugSettings
        {
            public Color PartitionGridColor = Color.white;
        }
    }
}
#endif