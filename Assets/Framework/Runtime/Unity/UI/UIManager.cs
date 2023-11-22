#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using System.Collections.Generic;
using UnityEngine;

namespace GBG.Framework.Unity.UI
{
    public partial class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField]
        private LoadingUIController _defaultLoadingUI;

        private List<Group> _groups = new();
        private Group _defaultGroup;
        private Group _topGroup;
        private List<IUIController> _closingUIList = new();


        public void Tick()
        {
            for (int i = _closingUIList.Count - 1; i >= 0; i--)
            {
                var ui = _closingUIList[i];
                if (ui.IsDestroyed())
                {
                    _closingUIList.RemoveAt(i);
                    continue;
                }

                if (ui.IsCloseEffectFinished())
                {
                    _closingUIList.RemoveAt(i);
                    IUIController.ProcessUIClosing(ui);
                }
            }
        }


        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();

            _defaultGroup = GetOrCreateGroupInternal(DefaultGroupName, DefaultGroupPriority);
            _topGroup = GetOrCreateGroupInternal(TopGroupName, TopGroupPriority);


            // test
            Show("test", _defaultLoadingUI);



            // Create Default Loading UI
            if (_defaultLoadingUI.transform.parent) // Not a prefab
            {
                _defaultLoadingUI.transform.SetParent(_topGroup.Root, false);
                _defaultLoadingUI.TrySetActive(true);
            }
            else
            {
                var defaultLoadingUI = Instantiate(_defaultLoadingUI, _topGroup.Root, false);
                defaultLoadingUI.TrySetActive(true);
                _defaultLoadingUI = defaultLoadingUI;
            }

            _defaultLoadingUI.Close(fadeOutTime: 0);
        }

        #endregion


        #region Group

        public static readonly string DefaultGroupName = "DefaultGroup";
        public static readonly short DefaultGroupPriority = 0;
        public static readonly string TopGroupName = "TopGroup";
        public static readonly short TopGroupPriority = short.MaxValue;

        private readonly IList<string> _excludedGroupName = new string[1];


        public bool HasGroup(string name)
        {
            foreach (var group in _groups)
            {
                if (group.Name?.Equals(name) ?? group.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public void TryCreateGroup(string name, sbyte priority)
        {
            GetOrCreateGroupInternal(name, priority);
        }

        private Group GetOrCreateGroupInternal(string name, short priority)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Debug.LogError("Create ui group failed. Group name can not be empty.");
                return null;
            }

            var groupIndex = -1;
            for (int i = 0; i < _groups.Count; i++)
            {
                var group = _groups[i];
                if (group.Name.Equals(name))
                {
                    //Debug.LogError($"UI group '{name}' already exist.");
                    return group;
                }

                if (groupIndex == -1 && group.Priority > priority)
                {
                    groupIndex = i;
                }
            }

            if (groupIndex == -1)
            {
                groupIndex = _groups.Count;
            }

            var groupRootGo = new GameObject($"{name}@{priority}", new System.Type[] { typeof(RectTransform) });
            var groupRoot = (RectTransform)groupRootGo.transform;
            groupRoot.SetParent(transform, false);
            groupRoot.SetSiblingIndex(groupIndex);
            groupRoot.anchorMin = Vector2.zero;
            groupRoot.anchorMax = Vector2.one;
            groupRoot.anchoredPosition = Vector2.zero;
            groupRoot.sizeDelta = Vector2.zero;

            var newGroup = new Group
            {
                Name = name,
                Priority = priority,
                Root = groupRoot,
            };
            _groups.Insert(groupIndex, newGroup);

            return newGroup;
        }

        public bool TryDeleteGroup(string name)
        {
            var group = FindGroup(name, out var groupIndex);
            if (groupIndex == -1)
            {
                //Debug.LogError($"Delete ui group '{name}' failed. Group does not exist.");
                return false;
            }

            if (group.UIList.Count > 0)
            {
                //Debug.LogError($"Delete ui group '{name}' failed. Group is not empty.");
                return false;
            }

            _groups.RemoveAt(groupIndex);
            Destroy(group.Root.gameObject);

            return true;
        }

        public void PauseGroups(string excludedGroupName)
        {
            _excludedGroupName[0] = excludedGroupName;
            PauseGroups(_excludedGroupName);
        }

        public void PauseGroups(IList<string> excludedGroupNames)
        {
            foreach (var group in _groups)
            {
                if (excludedGroupNames == null || !excludedGroupNames.Contains(group.Name))
                {
                    group.Pause();
                }
            }
        }

        public void ResumeGroups(string excludedGroupName)
        {
            _excludedGroupName[0] = excludedGroupName;
            ResumeGroups(_excludedGroupName);
        }

        public void ResumeGroups(IList<string> excludedGroupNames)
        {
            foreach (var group in _groups)
            {
                if (excludedGroupNames == null || !excludedGroupNames.Contains(group.Name))
                {
                    group.Resume();
                }
            }
        }

        private Group FindGroup(string name, out int index)
        {
            for (int i = 0; i < _groups.Count; i++)
            {
                Group group = _groups[i];
                if (group.Name?.Equals(name) ?? group.Name == name)
                {
                    index = i;
                    return group;
                }
            }

            index = -1;
            return null;
        }

        #endregion


        #region UI

        public IUIController Get(string uiName, string groupName = null)
        {
            GetGroupAndUI(groupName, uiName, out _, out var ui);
            return ui;
        }

        private void GetGroupAndUI(string groupName, string uiName, out Group group, out IUIController ui)
        {
            if (string.IsNullOrEmpty(groupName) || groupName.Equals(DefaultGroupName))
            {
                group = _defaultGroup;
            }
            else if (groupName.Equals(TopGroupName))
            {
                group = _topGroup;
            }
            else
            {
                group = FindGroup(groupName, out _);
            }

            ui = group?.Get(uiName);
        }

        public void Show<T>(string uiName, T uiPrefab, string groupName = null) where T : MonoBehaviour, IUIController
        {
            GetGroupAndUI(groupName, uiName, out var group, out var ui);

            if (ui != null)
            {
                ui.Show();
                return;
            }

            if (group == null)
            {
                Debug.LogError($"Show ui '{uiName}' failed. Group '{groupName}' does not exist.");
                return;
            }

            ui = Instantiate(uiPrefab, group.Root);
            ui.Name = uiName;
            ui.Closed += OnUIClosed;
            group.Add(ui, out var siblingIndex);
            ui.SetSiblingIndex(siblingIndex);
            ui.Show();

            if (group.IsPaused)
            {
                ui.Pause();
            }
        }

        public void Show(string uiName, string uiResKey, string groupName = null)
        {
            Debug.LogError("TODO: Load ui by uiResKey is not implemented.");
        }

        public void Close(string uiName, string groupName = null)
        {
            var ui = Get(uiName, groupName);
            if (ui == null)
            {
                Debug.LogError($"Close ui failed. UI '{uiName}' does not exist in group '{groupName}'.");
                return;
            }

            ui.Close();
        }

        private void OnUIClosed(IUIController ui)
        {
            ui.Closed -= OnUIClosed;
            if (ui.CloseMode == UICloseMode.Custom)
            {
                return;
            }

            if (ui.IsCloseEffectFinished())
            {
                IUIController.ProcessUIClosing(ui);
                return;
            }

            _closingUIList.Add(ui);
        }

        #endregion


        #region Top UI

        public IUIController GetTop(string uiName) => Get(uiName: uiName, groupName: TopGroupName);
        public void ShowTop<T>(string uiName, T uiPrefab) where T : MonoBehaviour, IUIController<T> => Show(uiName: uiName, uiPrefab: uiPrefab, groupName: TopGroupName);
        public void ShowTop(string uiName, string uiResKey) => Show(uiName: uiName, uiResKey: uiResKey, groupName: TopGroupName);
        public void CloseTop(string uiName) => Close(uiName: uiName, groupName: TopGroupName);

        #endregion


        #region Default Loading UI

        public LoadingUIController DefaultLoadingUI { get => _defaultLoadingUI; set => _defaultLoadingUI = value; }

        public void ShowDefaultLoading(object locker = null, float? fadeInTime = null)
        {
            _defaultLoadingUI.Show(locker: locker, fadeInTime: fadeInTime);
        }

        public void CloseDefaultLoading(object locker = null, float? fadeOutTime = null)
        {
            _defaultLoadingUI.Close(locker: locker, fadeOutTime: fadeOutTime);
        }

        #endregion
    }
}
#endif