using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.ScenePartition.Editor
{
    public delegate SceneData[] SubScenesCollector(Scene rootSceneComp);

    public static class Utility
    {
        public static SubScenesCollector SubscenesCollector;

        private static bool CheckSubscenesCollector()
        {
            if (SubscenesCollector != null)
            {
                return true;
            }

            string message = $"Please assign `{typeof(Utility).FullName}.{nameof(SubscenesCollector)}` before collecting subscenes.";
            EditorUtility.DisplayDialog("Error", message, "Ok");

            return false;
        }

        public static SceneData[] CollectSubsceneWithSuffixPNumberInSameFolder(Scene rootScene)
        {
            string rootSceneFolder = Path.GetDirectoryName(rootScene.path);
            string[] subscenePaths = Directory.GetFiles(rootSceneFolder,
                $"{rootScene.name}_P*.unity", SearchOption.TopDirectoryOnly);
            SceneData[] result = new SceneData[subscenePaths.Length];
            for (int i = 0; i < subscenePaths.Length; i++)
            {
                string subscenePath = subscenePaths[i].Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string subsceneGuid = AssetDatabase.AssetPathToGUID(subscenePath);
                result[i] = new SceneData(subscenePath, subsceneGuid);
            }

            return result;
        }

        public static bool CollectSubscenes(RootScene rootSceneComp)
        {
            if (!CheckSubscenesCollector())
            {
                Debug.LogError($"Collect subscenes failed. `{typeof(EditorUtility).FullName}.{nameof(SubscenesCollector)}` is not assigned.");
                return false;
            }

            SceneData[] subscenes = SubscenesCollector(rootSceneComp.gameObject.scene);
            Bounds[] subsceneBoundsList = new Bounds[subscenes.Length];
            for (int i = 0; i < subscenes.Length; i++)
            {
                SceneData subsceneData = subscenes[i];
                string subscenePath = AssetDatabase.GUIDToAssetPath(subsceneData.Guid);
                Scene subscene = EditorSceneManager.OpenScene(subscenePath, OpenSceneMode.Additive);
                Bounds subsceneBounds = CalculateSceneBounds(subscene);
                subsceneBoundsList[i] = subsceneBounds;
            }

            Undo.RecordObject(rootSceneComp, "Collect Subscenes");
            CalculateScenePartition(rootSceneComp.PartitionData, subscenes, subsceneBoundsList);
            EditorUtility.SetDirty(rootSceneComp);

            return true;
        }

        public static bool LoadAllScenes(ref RootScene rootSceneComp)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return false;
            }

            var rootScenePath = rootSceneComp.gameObject.scene.path;
            var rootScene = EditorSceneManager.OpenScene(rootScenePath, OpenSceneMode.Single);
            SceneManager.SetActiveScene(rootScene);
            rootSceneComp = UObject.FindObjectOfType<RootScene>();
            foreach (SceneData subsceneData in rootSceneComp.PartitionData.Subscenes)
            {
                if (!subsceneData.IsValid())
                {
                    continue;
                }

                string subScenePath = AssetDatabase.GUIDToAssetPath(subsceneData.Guid);
                EditorSceneManager.OpenScene(subScenePath, OpenSceneMode.Additive);
            }

            Selection.activeGameObject = rootSceneComp.gameObject;

            return true;
        }

        public static void CalculateScenePartition(ScenePartitionData partitionData,
            SceneData[] rawSubscenes, Bounds[] rawSubsceneBoundsList)
        {
            Assert.IsTrue(rawSubscenes.Length == rawSubsceneBoundsList.Length);

            partitionData.Subscenes = new SceneData[partitionData.PartitionCount0 * partitionData.PartitionCount1];
            for (int i = 0; i < rawSubscenes.Length; i++)
            {
                SceneData subscene = rawSubscenes[i];
                Bounds subsceneBounds = rawSubsceneBoundsList[i];
                int subsceneIndex;
                switch (partitionData.PartitionType)
                {
                    case ScenePartitionType.XZ:
                        subsceneIndex = partitionData.GetSubsceneIndex(subsceneBounds.center.x, subsceneBounds.center.z);
                        break;

                    case ScenePartitionType.XY:
                        subsceneIndex = partitionData.GetSubsceneIndex(subsceneBounds.center.x, subsceneBounds.center.y);
                        break;

                    default:
                        throw new Exception($"Unknown scene partition type: {partitionData.PartitionType}.");
                }

                if (subsceneIndex == -1)
                {
                    SceneAsset subsceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(subscene.Guid));
                    Debug.LogError($"The center of subscene '{subsceneAsset.name}' is not in any partition.", subsceneAsset);
                    continue;
                }

                if (partitionData.Subscenes[subsceneIndex].IsValid())
                {
                    SceneAsset subsceneAsset0 = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(partitionData.Subscenes[subsceneIndex].Guid));
                    SceneAsset subsceneAsset1 = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(subscene.Guid));
                    Debug.LogError($"Subscene '{subsceneAsset0.name}' and subscene '{subsceneAsset1.name}' is in the same partition.", subsceneAsset0);
                    Debug.LogError($"Subscene '{subsceneAsset1.name}' and subscene '{subsceneAsset0.name}' is in the same partition.", subsceneAsset1);
                    continue;
                }

                partitionData.Subscenes[subsceneIndex] = subscene;
            }
        }

        public static Bounds CalculateSceneBounds(Scene scene)
        {
            Bounds sceneBounds = default;
            bool hasBounds = false;
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                {
                    if (hasBounds)
                    {
                        sceneBounds.Encapsulate(r.bounds);
                    }
                    else
                    {
                        sceneBounds = r.bounds;
                        hasBounds = true;
                    }
                }

                Collider[] colliders = go.GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    if (hasBounds)
                    {
                        sceneBounds.Encapsulate(collider.bounds);
                    }
                    else
                    {
                        sceneBounds = collider.bounds;
                        hasBounds = true;
                    }
                }

                Collider2D[] collider2Ds = go.GetComponentsInChildren<Collider2D>();
                foreach (Collider2D collider in collider2Ds)
                {
                    if (hasBounds)
                    {
                        sceneBounds.Encapsulate(collider.bounds);
                    }
                    else
                    {
                        sceneBounds = collider.bounds;
                        hasBounds = true;
                    }
                }
            }

            return sceneBounds;
        }
    }
}
