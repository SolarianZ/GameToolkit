#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBG.GameToolkit.Unity.UI
{
    //[RequireComponent(typeof(Graphic))]
    //[RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    //[DisallowMultipleComponent]
    public sealed partial class LoadingUIController : UIControllerBase
    {
        public enum State
        {
            FadingIn,
            FadeInComplete,
            FadingOut,
            FadeOutComplete,
        }

        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private float _defaultFadeInTime = 0.1f;
        [SerializeField]
        private AnimationCurve _fadeInCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField]
        private float _defaultFadeOutTime = 0.15f;
        [SerializeField]
        private AnimationCurve _fadeOutCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public float FadeInTime
        {
            get => _defaultFadeInTime;
            set => _defaultFadeInTime = value;
        }
        public float FadeOutTime
        {
            get => _defaultFadeOutTime;
            set => _defaultFadeOutTime = value;
        }
        public AnimationCurve FadeInCurve
        {
            get => _fadeInCurve;
            set
            {
                _fadeInCurve = value;
                CheckCurves();
            }
        }
        public AnimationCurve FadeOutCurve
        {
            get => _fadeOutCurve;
            set
            {
                _fadeOutCurve = value;
                CheckCurves();
            }
        }

        public State FadeState { get; private set; }
        public int LockCount => _lockers.Count;

        private readonly HashSet<object> _lockers = new();
        private float _fadeTime;
        private float _fadeTimer;

        public event Action OnFadeInComplete;
        public event Action OnFadeOutComplete;


        public void Tick(float deltaTime)
        {
            if (FadeState == State.FadingIn)
            {
                _fadeTimer += deltaTime;
                var alpha = _fadeInCurve.Evaluate(_fadeTimer / _fadeTime);
                _canvasGroup.alpha = alpha;
                _canvas.enabled = true;

                if (_fadeTimer > _fadeTime)
                {
                    _fadeTimer = 0;
                    _fadeTime = 0;
                    FadeState = State.FadeInComplete;
                    OnFadeInComplete?.Invoke();
                }
            }
            else if (FadeState == State.FadingOut)
            {
                _fadeTimer += deltaTime;
                var alpha = 1 - _fadeInCurve.Evaluate(_fadeTimer / _fadeTime);
                _canvasGroup.alpha = alpha;
                _canvas.enabled = true;

                if (_fadeTimer > _fadeTime)
                {
                    _fadeTimer = 0;
                    _fadeTime = 0;
                    _canvas.enabled = false;
                    FadeState = State.FadeOutComplete;
                    OnFadeOutComplete?.Invoke();
                }
            }
        }

        public void Show(object locker = null, float? fadeInTime = null)
        {
            if (locker != null)
            {
                _lockers.Add(locker);
            }

            if (FadeState == State.FadeInComplete)
            {
                return;
            }

            var newFadeTime = fadeInTime ?? _defaultFadeInTime;
            if (newFadeTime <= 0)
            {
                _canvasGroup.alpha = 1;
                _canvas.enabled = true;
                FadeState = State.FadeInComplete;
                OnFadeInComplete?.Invoke();
            }

            UpdateFadeTimes(newFadeTime, State.FadingIn);

            FadeState = State.FadingIn;

            ((IUIController)this).Show();
        }

        public void Close(object locker = null, float? fadeOutTime = null)
        {
            if (locker != null)
            {
                _lockers.Remove(locker);
            }

            if (FadeState == State.FadeOutComplete)
            {
                return;
            }

            if (LockCount > 0)
            {
                return;
            }

            var newFadeTime = fadeOutTime ?? _defaultFadeOutTime;
            if (newFadeTime <= 0)
            {
                _canvasGroup.alpha = 0;
                _canvas.enabled = false;
                FadeState = State.FadeOutComplete;
                OnFadeOutComplete?.Invoke();
            }

            UpdateFadeTimes(newFadeTime, State.FadingOut);

            FadeState = State.FadingOut;

            ((IUIController)this).Close();
        }

        public override bool IsCloseEffectFinished()
        {
            return FadeState == State.FadeOutComplete;
        }

        private void UpdateFadeTimes(float newFadeTime, State targetState)
        {
            var ratio = _fadeTime * newFadeTime;
            if (ratio <= 0 || _fadeTimer <= 0)
            {
                _fadeTimer = 0;
                _fadeTime = newFadeTime;
            }
            else
            {
                // keep fade progress
                _fadeTimer = _fadeTimer / _fadeTime * newFadeTime;
                _fadeTime = newFadeTime;
                if (FadeState != targetState)
                {
                    _fadeTimer = _fadeTime - _fadeTimer;
                }
            }
        }

        private void CheckCurves()
        {
            if (!_fadeInCurve.IsNormalized())
            {
                Debug.LogError("The fade in curve must start at (0,0) and end at (1,1).", this);
            }

            if (!_fadeOutCurve.IsNormalized())
            {
                Debug.LogError("The fade out curve must start at (0,0) and end at (1,1).", this);
            }
        }


        #region Unity Messages

        private void Reset()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            CheckCurves();

            FadeState = State.FadeOutComplete;
            _canvasGroup.alpha = 0;
            _canvas.enabled = false;
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        #endregion
    }
}
#endif