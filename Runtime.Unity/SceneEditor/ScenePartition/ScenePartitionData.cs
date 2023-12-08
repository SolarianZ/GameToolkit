using System;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ScenePartition
{
    public enum ScenePartitionType
    {
        XZ = 0,
        XY = 1,
        //ZY = 2,
    }

    [Serializable]
    public class ScenePartitionData
    {
        public ScenePartitionType PartitionType = ScenePartitionType.XZ;
        [Tooltip("The left bottom corner of the scene.")]
        public Vector3 SceneOrigin;
        [Min(0)]
        public int PartitionCount0;
        [Min(0)]
        public int PartitionCount1;
        [Min(0)]
        public float PartitionLength0 = 50; // x
        [Min(0)]
        public float PartitionLength1 = 50; // z or y
        public SceneData[] Subscenes = Array.Empty<SceneData>();


        public bool IsInSceneRange(float position0, float position1)
        {
            return GetSubsceneIndex(position0, position1) != -1;
        }

        public (int index0, int index1) GetSubsceneIndex2D(float position0, float position1)
        {
            int index0, index1;
            switch (PartitionType)
            {
                case ScenePartitionType.XZ:
                    if (position0 < SceneOrigin.x || position1 < SceneOrigin.z)
                    {
                        return (-1, -1);
                    }

                    index0 = (int)((position0 - SceneOrigin.x) / PartitionLength0);
                    if (index0 >= PartitionCount0)
                    {
                        return (-1, -1);
                    }

                    index1 = (int)((position1 - SceneOrigin.z) / PartitionLength1);
                    if (index1 >= PartitionCount1)
                    {
                        return (-1, -1);
                    }

                    break;

                case ScenePartitionType.XY:
                    if (position0 < SceneOrigin.x || position1 < SceneOrigin.y)
                    {
                        return (-1, -1);
                    }

                    index0 = (int)((position0 - SceneOrigin.x) / PartitionLength0);
                    if (index0 >= PartitionCount0)
                    {
                        return (-1, -1);
                    }

                    index1 = (int)((position1 - SceneOrigin.y) / PartitionLength1);
                    if (index1 >= PartitionCount1)
                    {
                        return (-1, -1);
                    }

                    break;

                default:
                    throw new Exception($"Unknown scene partition type: {PartitionType}.");
            }

            return (index0, index1);
        }

        public int GetSubsceneIndex(float position0, float position1)
        {
            (int index0, int index1) = GetSubsceneIndex2D(position0, position1);
            if (index0 == -1 || index1 == -1)
            {
                return -1;
            }

            int index = PartitionCount0 * index1 + index0;
            return index;
        }

        public SceneData GetSubsceneData(float position0, float position1)
        {
            int index = GetSubsceneIndex(position0, position1);
            if (index == -1)
            {
                return default;
            }

            SceneData sceneData = Subscenes[index];
            return sceneData;
        }

        public SceneData[,] CreateSubsceneMatrix()
        {
            SceneData[,] matrix = new SceneData[PartitionCount0, PartitionCount1];
            for (int j = 0; j < PartitionCount1; j++)
            {
                for (int i = 0; i < PartitionCount0; i++)
                {
                    int index = PartitionCount0 * j + i;
                    matrix[i, j] = Subscenes[index];
                }
            }

            return matrix;
        }
    }
}
