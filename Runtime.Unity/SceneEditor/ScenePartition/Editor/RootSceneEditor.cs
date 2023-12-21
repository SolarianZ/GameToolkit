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
                Utility.GenerateLighting(ref rootSceneComp);
            }
        }
    }
}
