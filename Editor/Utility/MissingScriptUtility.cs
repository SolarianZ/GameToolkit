using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UDebug = UnityEngine.Debug;

namespace GBG.GameToolkit.Editor.Utility
{
    public static class MissingScriptUtility
    {
        [MenuItem("Tools/Bamboo/GameObject/Log GameObjects with Missing Script in Loaded Scenes")]
        public static void LogGameObjectsWithMissingScriptInLoadedScenes()
        {
            var missingScriptCount = 0;
            var sceneCount = SceneManager.sceneCount;
            for (var i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                {
                    continue;
                }

                var rootGameObjects = scene.GetRootGameObjects();
                foreach (var rootGameObject in rootGameObjects)
                {
                    missingScriptCount += LogMissingScriptsOnGameObjectRecursively(rootGameObject);
                }
            }

            UDebug.Log($"Found {missingScriptCount} missing script(s) in loaded scene(s).");
        }

        public static int LogMissingScriptsOnGameObjectRecursively(GameObject rootGameObject)
        {
            var missingScriptCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(rootGameObject);
            if (missingScriptCount > 0)
            {
                UDebug.Log($"Found {missingScriptCount} missing script(s) on {rootGameObject.name}.", rootGameObject);
            }

            foreach (Transform child in rootGameObject.transform)
            {
                missingScriptCount += LogMissingScriptsOnGameObjectRecursively(child.gameObject);
            }

            return missingScriptCount;
        }

        [MenuItem("Tools/Bamboo/GameObject/Remove Missing Scripts From Selected GameObjects Recursively")]
        [MenuItem("GameObject/Bamboo/Remove Missing Scripts Recursively")]
        public static void RemoveMissingScriptsFromSelectedGameObjectsRecursively()
        {
            var selectedGameObjects = Selection.gameObjects;
            foreach (var gameObject in selectedGameObjects)
            {
                Undo.RegisterFullObjectHierarchyUndo(gameObject, nameof(RemoveMissingScriptsRecursively));
                RemoveMissingScriptsRecursively(gameObject);
            }
        }

        [MenuItem("Tools/Bamboo/GameObject/Remove Missing Scripts From Selected GameObjects Recursively", validate = true)]
        [MenuItem("GameObject/Bamboo/Remove Missing Scripts Recursively", validate = true)]
        public static bool RemoveMissingScriptsFromSelectedGameObjectsRecursivelyValidate()
        {
            return Selection.gameObjects.Length > 0;
        }

        public static void RemoveMissingScriptsRecursively(this GameObject gameObject)
        {
            var removedCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            if (removedCount > 0)
            {
                UDebug.Log($"Removed {removedCount} missing script(s) from {gameObject.name}.", gameObject);
            }

            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.RemoveMissingScriptsRecursively();
            }
        }
    }
}
