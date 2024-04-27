using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public partial class AssetCheckerWindow : EditorWindow
    {
        static class Info
        {
            public const string SettingsFieldName = "SettingsField";
            public const string SettingsPropertyPath = "_settings";
            public const string CreateSettingsAssetButtonName = "CreateSettingsAssetButton";
            public const string ExecuteButtonName = "ExecuteButton";
        }

        [MenuItem("Tools/Bamboo/Asset Checker")]
        public static void Open()
        {
            GetWindow<AssetCheckerWindow>("AssetCheckerWindow");
        }


        [SerializeField]
        private VisualTreeAsset _mainVisualTreeAsset;


        #region Controls

        private ObjectField _settingsField;
        private Button _executeButton;

        #endregion

        private void OnSettingsObjectChanged(ChangeEvent<Object> evt)
        {
            _executeButton.SetEnabled(_settings);
        }

        private void ExecuteChecker()
        {
            Assert.IsTrue(_settings);

            Debug.LogError(_settings);
        }
    }
}