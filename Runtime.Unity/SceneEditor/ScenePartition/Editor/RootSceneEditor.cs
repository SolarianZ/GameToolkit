﻿#if UNITY_EDITOR // Fix compile errors on build
using GBG.GameToolkit.Unity.ScenePartition;
using UnityEditor;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Unity.Editor.ScenePartition
{
    [CustomEditor(typeof(RootScene))]
    public class RootSceneEditor : ValidatableEditor
    {
        protected override ListView ValidationResultListView { get; set; }


        public override VisualElement CreateInspectorGUI()
        {
            EditorValidationUtility.CreateValidationResultViewsAndDefaultInspector(serializedObject, this,
                out var rootContainer, out _,
                out var validationResultListView, out _);
            ValidationResultListView = validationResultListView;

            var collectSubscenesButton = new Button(CollectSubScenes)
            {
                name = "CollectSubscenesButton",
                text = "Collect Subscenes",
                style =
                {
                    marginTop = 10,
                }
            };
            collectSubscenesButton.AddToClassList("collect-subscenes-button");
            rootContainer.Add(collectSubscenesButton);

            var loadAllScenesButton = new Button(LoadAllScenes)
            {
                name = "LoadAllScenesButton",
                text = "Load All Scenes",
            };
            loadAllScenesButton.AddToClassList("load-all-scenes-button");
            rootContainer.Add(loadAllScenesButton);

            var generateLightingButton = new Button(GenerateLighting)
            {
                name = "GenerateLightingButton",
                text = "Generate Lighting",
            };
            generateLightingButton.AddToClassList("generate-lighting-button");
            rootContainer.Add(generateLightingButton);

            return rootContainer;
        }

        private void CollectSubScenes()
        {
            EditorScenePartitionUtility.CollectSubscenes((RootScene)target);
        }

        private void LoadAllScenes()
        {
            RootScene rootSceneComp = (RootScene)target;
            EditorScenePartitionUtility.LoadAllScenes(ref rootSceneComp);
        }

        private void GenerateLighting()
        {
            RootScene rootSceneComp = (RootScene)target;
            EditorScenePartitionUtility.GenerateLighting(ref rootSceneComp);
        }
    }
}
#endif