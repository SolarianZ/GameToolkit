#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using System;
using System.Reflection;
using UnityEngine;

namespace GBG.GameToolkit.Unity.UI
{
    public static class UIUtility
    {
        #region Position

        public static void SetScreenPosition(this RectTransform rectTransform, Vector2 screenPosition, Canvas canvas, float? z = null)
        {
            if (!canvas)
            {
                canvas = rectTransform.GetComponentInParent<Canvas>();
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera)
            {
                Vector3 screenPos3D = screenPosition;
                screenPos3D.z = z ?? canvas.planeDistance;
                rectTransform.position = canvas.worldCamera.ScreenToWorldPoint(screenPos3D);
                return;
            }

            var uiPos = new Vector3(screenPosition.x, screenPosition.y, z ?? 0);
            rectTransform.position = uiPos;
        }

        public static void SetViewportPosition(this RectTransform rectTransform, Vector2 viewportPosition, Canvas canvas, float? z = null)
        {
            if (!canvas)
            {
                canvas = rectTransform.GetComponentInParent<Canvas>();
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera)
            {
                Vector3 viewportPos3D = viewportPosition;
                viewportPos3D.z = z ?? canvas.planeDistance;
                var worldPos = canvas.worldCamera.ViewportToWorldPoint(viewportPos3D);
                rectTransform.position = worldPos;
                return;
            }

            var gameViewSize = GetMainGameViewSize();
            var uiPos = new Vector3(viewportPosition.x * gameViewSize.x, viewportPosition.y * gameViewSize.y, z ?? 0);
            rectTransform.position = uiPos;
        }

        public static Vector2 GetScreenPosition(this RectTransform rectTransform, Canvas canvas)
        {
            if (!canvas)
            {
                canvas = rectTransform.GetComponentInParent<Canvas>();
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera)
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTransform.position);
                return screenPos;
            }
            else
            {
                var uiPos = rectTransform.position;
                return uiPos;
            }
        }

        public static Vector2 GetViewportPosition(this RectTransform rectTransform, Canvas canvas)
        {
            if (!canvas)
            {
                canvas = rectTransform.GetComponentInParent<Canvas>();
            }

            var gameViewSize = GetMainGameViewSize();
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera)
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTransform.position);
                var x = screenPos.x / gameViewSize.x;
                var y = screenPos.y / gameViewSize.y;
                return new Vector2(x, y);
            }
            else
            {
                var uiPos = rectTransform.position;
                var x = uiPos.x / gameViewSize.x;
                var y = uiPos.y / gameViewSize.y;
                return new Vector2(x, y);
            }
        }

        #endregion


        #region Size

#if UNITY_EDITOR
        private static Func<Vector2> _EDITOR_GetMainGameViewSize;
#endif

        public static Vector2 GetMainGameViewSize(bool createDelegate = true)
        {
#if UNITY_EDITOR
            if (_EDITOR_GetMainGameViewSize != null)
            {
                return _EDITOR_GetMainGameViewSize();
            }

            var playModeViewType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.PlayModeView");
            var getMainSizeMethod = playModeViewType.GetMethod("GetMainPlayModeViewTargetSize",
                BindingFlags.NonPublic | BindingFlags.Static);
            if (createDelegate)
            {
                _EDITOR_GetMainGameViewSize = (Func<Vector2>)getMainSizeMethod.CreateDelegate(typeof(Func<Vector2>));
                return _EDITOR_GetMainGameViewSize();
            }

            var mainSize = (Vector2)getMainSizeMethod.Invoke(null, null);
            return mainSize;
#else
            return new Vector2(Screen.width, Screen.height);
#endif
        }

        public static void SetViewportSize(this RectTransform rectTransform, Vector2 viewportSize, Canvas canvas)
        {
            if (!canvas)
            {
                canvas = rectTransform.GetComponentInParent<Canvas>();
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera)
            {
                var uiCam = canvas.worldCamera;
                rectTransform.sizeDelta = new Vector2(uiCam.pixelWidth * viewportSize.x, uiCam.pixelHeight * viewportSize.y);
                return;
            }

            var gameViewSize = GetMainGameViewSize();
            rectTransform.sizeDelta = new Vector2(gameViewSize.x * viewportSize.x, gameViewSize.y * viewportSize.y);
        }

        #endregion
    }
}
#endif