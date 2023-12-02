#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBG.GameToolkit.Unity.UI
{
    partial class UIManager
    {
        [Serializable]
        class Group
        {
            public string Name;
            public short Priority;
            public RectTransform Root;
            public bool IsPaused;
            public List<IUIController> UIList = new();
            public Dictionary<string, IUIController> UITable = new();

            public void Pause()
            {
                if (IsPaused)
                {
                    return;
                }

                IsPaused = true;

                foreach (var ui in UIList)
                {
                    ui.Pause();
                }
            }

            public void Resume()
            {
                if (!IsPaused)
                {
                    return;
                }

                IsPaused = false;

                foreach (var ui in UIList)
                {
                    ui.Resume();
                }
            }


            public IUIController Get(string uiName)
            {
                if (UITable.TryGetValue(uiName, out var ui))
                {
                    return ui;
                }

                return null;
            }

            public bool Add(IUIController ui, out int siblingIndex)
            {
                if (!UITable.TryAdd(ui.Name, ui))
                {
                    siblingIndex = -1;
                    Debug.LogError($"Add ui failed. There is already a ui named '{ui.Name}'.");
                    return false;
                }

                ui.Destroyed += OnUIDestroyed;

                for (int i = UIList.Count - 1; i >= 0; i--)
                {
                    var prevUI = UIList[i];
                    if (prevUI.Priority <= ui.Priority)
                    {
                        siblingIndex = i + 1;
                        UIList.Insert(siblingIndex, ui);
                        return true;
                    }
                }

                siblingIndex = 0;
                UIList.Insert(0, ui);

                return true;
            }

            public bool Remove(string uiName)
            {
                if (UITable.Remove(uiName, out var ui))
                {
                    UIList.Remove(ui);
                    ui.Destroyed -= OnUIDestroyed;

                    return true;
                }

                return false;
            }

            private void OnUIDestroyed(IUIController ui)
            {
                ui.Destroyed -= OnUIDestroyed;
                if (UITable.Remove(ui.Name, out _))
                {
                    UIList.Remove(ui);
                }
            }
        }
    }
}
#endif