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
        string Name { get; internal set; }
        int Priority { get; }
        UIState State { get; internal set; }
        UICloseMode CloseMode { get; }

        event Action<IUIController> Closed;
        event Action<IUIController> Destroyed;


        void Show();
        void Resume();
        void Pause();
        void Close();

        bool IsCloseEffectFinished();

        void SetSiblingIndex(int index);
        void Deactivate();
        void Destroy();
        bool IsDestroyed();

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
            public int priority;
            public UICloseMode closeMode;
        }

        [SerializeField]
        protected BasicSettings basicSettings = new();

        string IUIController.Name { get; set; }
        int IUIController.Priority => basicSettings.priority;
        UICloseMode IUIController.CloseMode => basicSettings.closeMode;
        UIState IUIController.State { get => UIState; set => UIState = value; }
        public UIState UIState { get; private set; }

        public event Action<IUIController> Closed;
        public event Action<IUIController> Destroyed;


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
                //Debug.LogError($"Can not pause a UI that is in '{UIState}' state.", this);
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
                //Debug.LogError($"Can not resume a UI that is in '{UIState}' state.", this);
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

            Closed?.Invoke(this);
        }

        public abstract bool IsCloseEffectFinished();


        void IUIController.SetSiblingIndex(int index) => transform.SetSiblingIndex(index);
        void IUIController.Deactivate() => gameObject.TrySetActive(false);
        void IUIController.Destroy() => Destroy(gameObject);
        bool IUIController.IsDestroyed() => !gameObject;


        protected virtual void OnShow() { }
        protected virtual void OnPause() { }
        protected virtual void OnResume() { }
        protected virtual void OnClose() { }

        protected virtual void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }
    }
}
#endif