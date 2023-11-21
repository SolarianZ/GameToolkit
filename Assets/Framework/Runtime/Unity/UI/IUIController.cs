#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS

using System;
using UnityEngine;

namespace GBG.Framework.Unity.UI
{
    public enum UIState
    {
        Closed,
        Active,
        Paused,
        Closing,
    }

    public enum UICloseMode
    {
        Destroy,
        Deactivate,
        Custom,
    }

    public interface IUIController
    {
        bool IsFullScreenOpaque { get; }
        UIState State { get; internal set; }
        UICloseMode CloseMode { get; }

        void Show();
        void Resume();
        void Pause();
        void Close();

        bool IsCloseEffectFinished();

        void Deactivate();
        void Destroy();

        static bool ProcessUIClosing(IUIController ui)
        {
            if (ui.State == UIState.Closing && ui.IsCloseEffectFinished())
            {
                switch (ui.CloseMode)
                {
                    case UICloseMode.Destroy:
                        ui.State = UIState.Closed;
                        ui.Destroy();
                        break;

                    case UICloseMode.Deactivate:
                        ui.State = UIState.Closed;
                        ui.Deactivate();
                        break;

                    case UICloseMode.Custom:
                        ui.State = UIState.Closed;
                        break;

                    default:
                        throw new Exception($"Unknown ui state: {ui.State}.");
                }
            }

            return ui.State == UIState.Closed;
        }
    }

    public interface IUIController<T> : IUIController where T : MonoBehaviour, IUIController<T> { }

    public abstract class UIControllerBase : MonoBehaviour, IUIController<UIControllerBase>
    {
        [Serializable]
        public class BasicSettings
        {
            public bool isFullScreenOpaque;
            public UICloseMode closeMode;
        }

        [SerializeField]
        protected BasicSettings basicSettings = new();

        bool IUIController.IsFullScreenOpaque => basicSettings.isFullScreenOpaque;
        UICloseMode IUIController.CloseMode => basicSettings.closeMode;
        UIState IUIController.State { get => UIState; set => UIState = value; }
        public UIState UIState { get; private set; }


        void IUIController.Show()
        {
            if (UIState == UIState.Active)
            {
                return;
            }

            if (UIState == UIState.Paused)
            {
                ((IUIController)this).Resume();
                return;
            }

            switch (basicSettings.closeMode)
            {
                case UICloseMode.Deactivate:
                    gameObject.TrySetActive(true);
                    break;
                case UICloseMode.Destroy:
                case UICloseMode.Custom:
                    break;
                default:
                    throw new Exception($"Unknown ui close mode: {basicSettings.closeMode}.");
            }

            UIState = UIState.Active;
            OnShow();
        }

        void IUIController.Pause()
        {
            if (UIState == UIState.Paused)
            {
                return;
            }

            if (UIState == UIState.Closing || UIState == UIState.Closed)
            {
                Debug.LogError($"Can not pause a UI that is in '{UIState}' state.", this);
                return;
            }

            UIState = UIState.Paused;
            OnPause();
        }

        void IUIController.Resume()
        {
            if (UIState == UIState.Active)
            {
                return;
            }

            if (UIState == UIState.Closing || UIState == UIState.Closed)
            {
                Debug.LogError($"Can not resume a UI that is in '{UIState}' state.", this);
                return;
            }

            UIState = UIState.Active;
            OnResume();
        }

        void IUIController.Close()
        {
            if (UIState == UIState.Closed || UIState == UIState.Closing)
            {
                return;
            }

            UIState = UIState.Closing;
            OnClose();

            IUIController.ProcessUIClosing(this);
        }

        public abstract bool IsCloseEffectFinished();


        void IUIController.Deactivate() => gameObject.TrySetActive(false);
        void IUIController.Destroy() => Destroy(gameObject);


        protected virtual void OnShow() { }
        protected virtual void OnPause() { }
        protected virtual void OnResume() { }
        protected virtual void OnClose() { }
    }
}
#endif