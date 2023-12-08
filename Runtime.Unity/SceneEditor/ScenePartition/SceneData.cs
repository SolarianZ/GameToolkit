using System;

namespace GBG.GameToolkit.Unity.ScenePartition
{
    [Serializable]
    public struct SceneData
    {
        public string ResKey;
        public string Guid;

        public SceneData(string resKey, string guid)
        {
            ResKey = resKey;
            Guid = guid;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(ResKey))
            {
                return false;
            }

#if UNITY_EDITOR
            if (string.IsNullOrEmpty(Guid))
            {
                return false;
            }
#endif

            return true;
        }

        public override string ToString()
        {
            return $"{ResKey} ({Guid})";
        }
    }
}
