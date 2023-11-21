#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBG.Framework.Unity.UI
{
    public partial class UIManager : MonoSingleton<UIManager>
    {
        public static readonly string DefaultGroupName = "DefaultGroup";
        public static readonly short DefaultGroupPriority = 0;
        public static readonly string TopGroupName = "TopGroup";
        public static readonly short TopGroupPriority = short.MaxValue;

        [SerializeField]
        private RectTransform _generalNode;
        [SerializeField]
        private RectTransform _topNode;
        [SerializeField]
        private LoadingUIController _defaultLoadingUI;

        //private readonly Dictionary<string, IUIController> _uiTable = new();
        //private readonly Dictionary<string, Group> _groups = new();
        private List<Group> _groups = new();


        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();

            CreateGroupInternal(DefaultGroupName, DefaultGroupPriority);
            CreateGroupInternal(TopGroupName, TopGroupPriority);
        }

        #endregion


        #region Group

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

        public void CreateGroup(string name, sbyte priority)
        {
            CreateGroupInternal(name, priority);
        }

        private void CreateGroupInternal(string name, short priority)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Debug.LogError("Create ui group failed. Group name can not be empty.");
                return;
            }

            var groupIndex = -1;
            for (int i = 0; i < _groups.Count; i++)
            {
                var group = _groups[i];
                if (group.Name.Equals(name))
                {
                    Debug.LogError($"UI group '{name}' already exist.");
                    return;
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
        }

        public bool DeleteGroup(string name)
        {
            var groupRoot = FindGroupRoot(name, out var groupIndex);
            if (groupIndex == -1)
            {
                Debug.LogError($"Delete ui group '{name}' failed. Group not exist.");
                return false;
            }

            if (groupRoot.childCount > 0)
            {
                Debug.LogError($"Delete ui group '{name}' failed. Group is not empty.");
                return false;
            }

            _groups.RemoveAt(groupIndex);
            Destroy(groupRoot.gameObject);

            return true;
        }

        private RectTransform FindGroupRoot(string name, out int index)
        {
            for (int i = 0; i < _groups.Count; i++)
            {
                Group group = _groups[i];
                if (group.Name?.Equals(name) ?? group.Name == name)
                {
                    index = i;
                    return group.Root;
                }
            }

            index = -1;
            return null;
        }

        #endregion


        public IUIController Get(string uiName) { return null; }

        public void Show<T>(string uiName, T prefab) where T : MonoBehaviour, IUIController<T> { }

        public void Show(string uiName) { }

        public void Close(string uiName) { }

        public IUIController GetTop(string uiName) { return null; }

        public void ShowTop<T>(string uiName, T prefab) where T : MonoBehaviour, IUIController<T> { }

        public void ShowTop(string uiName) { }

        public void CloseTop(string uiName) { }

        public void ShowDefaultLoading(object locker = null, float? fadeInTime = null)
        {
            _defaultLoadingUI.Show(locker: locker, fadeInTime: fadeInTime);
        }

        public void CloseDefaultLoading(object locker = null, float? fadeOutTime = null)
        {
            _defaultLoadingUI.Close(locker: locker, fadeOutTime: fadeOutTime);
        }

        [Serializable]
        struct Group
        {
            public string Name;
            public short Priority;
            public RectTransform Root;
        }
    }
}
#endif