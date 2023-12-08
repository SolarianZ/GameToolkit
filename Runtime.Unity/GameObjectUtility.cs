using UnityEngine;

namespace GBG.GameToolkit.Unity
{
    public static class GameObjectUtility
    {
        public static void TrySetActive(this Component component, bool active)
        {
            component.gameObject.TrySetActive(active);
        }

        public static void TrySetActive(this GameObject go, bool active)
        {
            if (go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}
