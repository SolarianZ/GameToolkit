#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using UnityEngine;

namespace GBG.Framework.Unity
{
    public static class UIUtils
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

            var uiPos = new Vector3(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height, z ?? 0);
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

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera)
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTransform.position);
                var x = screenPos.x / Screen.width;
                var y = screenPos.y / Screen.height;
                return new Vector2(x, y);
            }
            else
            {
                var uiPos = rectTransform.position;
                var x = uiPos.x / Screen.width;
                var y = uiPos.y / Screen.height;
                return new Vector2(x, y);
            }
        }

        #endregion


        #region Size

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

            rectTransform.sizeDelta = new Vector2(Screen.width * viewportSize.x, Screen.height * viewportSize.y);
        }

        #endregion
    }
}
#endif