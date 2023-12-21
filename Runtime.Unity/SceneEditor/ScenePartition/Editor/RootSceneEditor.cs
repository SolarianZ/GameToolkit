using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBG.GameToolkit.Unity.ScenePartition.Editor
{
    [CustomEditor(typeof(RootScene))]
    public class RootSceneEditor : UnityEditor.Editor
    {
        // TODO: Use UIToolkit and ValidatableEditor
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                return;
            }

            EditorGUILayout.Space();

            RootScene rootSceneComp = (RootScene)target;
            if (GUILayout.Button("Collect Subscenes"))
            {
                // TEST
                //Utility.SubscenesCollector = Utility.CollectSubsceneWithSuffixPNumberInSameFolder;
                Utility.CollectSubscenes(rootSceneComp);
            }

            if (GUILayout.Button("Load All Scenes"))
            {
                Utility.LoadAllScenes(ref rootSceneComp);
            }

            if (GUILayout.Button("Generate Lighting"))
            {
                GenerateLighting(ref rootSceneComp);
            }
        }

        private void GenerateLighting(ref RootScene rootSceneComp)
        {
            if (!EditorUtility.DisplayDialog("Warning",
                "This operation will bake the lighting data for all the scene partitions and may take a long time. Do you want to continue?",
                "Continue", "Cancel"))
            {
                return;
            }

            Utility.LoadAllScenes(ref rootSceneComp);
            string[] scenePaths = new string[SceneManager.loadedSceneCount];
            for (int i = 0; i < SceneManager.loadedSceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                scenePaths[i] = scene.path;
            }

            //Lightmapping.ClearLightingDataAsset();
            Lightmapping.BakeMultipleScenes(scenePaths);
        }
    }
}
