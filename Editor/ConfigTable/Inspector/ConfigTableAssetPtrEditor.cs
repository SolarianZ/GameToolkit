using GBG.GameToolkit.Unity.ConfigData;
using GBG.GameToolkit.Unity.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace GBG.GameToolkit.Editor.ConfigData
{
    [CustomEditor(typeof(ConfigListAssetPtr), true)]
    public class ConfigTableAssetPtrEditor : ValidatableEditor
    {
        protected override ListView ValidationResultListView { get; set; }


        public override VisualElement CreateInspectorGUI()
        {
            EditorValidationUtility.CreateValidationResultViewsAndDefaultInspector(serializedObject, this,
                out var rootContainer, out _,
                out var validationResultListView, out _);
            ValidationResultListView = validationResultListView;

            // Distinct
            Button distinctButton = new Button(DistinctConfigs)
            {
                name = "DistinctConfigsButton",
                text = "Distinct Configs",
                style =
                {
                    marginTop = 10,
                }
            };
            distinctButton.AddToClassList("distinct-configs-button");
            rootContainer.Add(distinctButton);

            // Delete multi
            TextField deleteMultiIdField = new TextField
            {
                name = "DeleteMultiIdListField",
                multiline = true,
                style =
                {
                    marginTop = 10,
                }
            };
            rootContainer.Add(deleteMultiIdField);
            Button deleteMultiConfigsButton = new Button(() => DeleteMultiConfigs(deleteMultiIdField.text))
            {
                name = "DeleteMultiConfigsButton",
                text = "Delete Multi Configs",
            };
            rootContainer.Add(deleteMultiConfigsButton);

            // Delete range
            VisualElement deleteRangeIdContainer = new VisualElement
            {
                name = "DeleteRangeIdContainer",
                style =
                {
                    marginTop = 10,
                    flexDirection = FlexDirection.Row,
                }
            };
            rootContainer.Add(deleteRangeIdContainer);
            IntegerField startIdField = new IntegerField
            {
                name = "StartIdField",
                value = 0,
                style =
                {
                    flexGrow = 1,
                    marginRight = 0,
                }
            };
            deleteRangeIdContainer.Add(startIdField);
            Label deleteRangeIdHyphenLabel = new Label
            {
                name = "DeleteRangeIdHyphenLabel",
                text = "-",
                style =
                {
                    paddingLeft = 10,
                    paddingRight = 10,
                }
            };
            deleteRangeIdContainer.Add(deleteRangeIdHyphenLabel);
            IntegerField endIdField = new IntegerField
            {
                name = "EndIdField",
                value = 0,
                style =
                {
                    flexGrow = 1,
                    marginLeft = 0,
                }
            };
            deleteRangeIdContainer.Add(endIdField);
            Button deleteRangeConfigsButton = new Button(() => DeleteRangeConfigs(startIdField.value, endIdField.value))
            {
                name = "DeleteRangeConfigsButton",
                text = "Delete Range Configs",
            };
            rootContainer.Add(deleteRangeConfigsButton);


            return rootContainer;
        }

        private void DistinctConfigs()
        {
            EditorConfigAssetUtility.DistinctConfigs((ConfigListAssetPtr)target);
        }

        private void DeleteMultiConfigs(string idListString)
        {
            string[] idStrList = idListString.Split(new char[] { ' ', ',', '，', ';', '\n' },
                StringSplitOptions.RemoveEmptyEntries);
            List<int> idList = new List<int>(idStrList.Length);
            foreach (string idStr in idStrList)
            {
                if (int.TryParse(idStr, out int id))
                {
                    idList.Add(id);
                }
            }

            EditorConfigAssetUtility.DeleteMultiConfigs((ConfigListAssetPtr)target, idList);
        }

        private void DeleteRangeConfigs(int startId, int endId)
        {
            EditorConfigAssetUtility.DeleteRangeConfigs((ConfigListAssetPtr)target, startId, endId);
        }
    }
}
