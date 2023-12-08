using UnityEditor;
using UnityEngine;

namespace GBG.GameToolkit.Unity.ScenePartition.Editor
{
    [CustomEditor(typeof(RootScene))]
    public class RootSceneEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
                return;

            EditorGUILayout.Space();

            RootScene rootSceneComp = (RootScene)target;
            if (GUILayout.Button("Collect Subscenes"))
            {
                // TODO: TEST
                //Utility.SubscenesCollector = Utility.CollectSubsceneWithSuffixPNumberInSameFolder;
                Utility.CollectSubscenes(rootSceneComp);
            }

            if (GUILayout.Button("Load All Scenes"))
            {
                Utility.LoadAllScenes(ref rootSceneComp);
            }
        }
    }
}
