﻿#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS

using UnityEngine;

namespace GBG.Framework.Unity
{
    public static class GameObjectUtils
    {
        public static void TrySetActive(this GameObject go, bool active)
        {
            if (go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}
#endif