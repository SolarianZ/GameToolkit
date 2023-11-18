#if UNITY_PIPELINE_URP
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UCamera = UnityEngine.Camera;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GBG.Framework.Unity.URP
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UCamera))]
    [RequireComponent(typeof(UniversalAdditionalCameraData))]
    public sealed class CameraOverlayController : MonoBehaviour
    {
        public string cameraId = "MainCamera";
        public string baseCameraId = "MainCamera";

        private UCamera _camera;
        private UniversalAdditionalCameraData _cameraData;
        private static readonly Dictionary<string, HashSet<UniversalAdditionalCameraData>> _baseCameras = new();
        private static readonly Dictionary<string, HashSet<UCamera>> _overlayCameras = new();


        private void RefreshStacks()
        {
            // Unity will automatically remove invalid cameras from the stack

            foreach (var kv in _overlayCameras)
            {
                var baseCamId = kv.Key;
                var overlayCams = kv.Value;
                if (overlayCams.Count == 0)
                {
                    continue;
                }

                if (_baseCameras.TryGetValue(baseCamId, out var baseCams))
                {
                    foreach (var baseCam in baseCams)
                    {
                        foreach (var overlayCam in overlayCams)
                        {
                            if (!baseCam.cameraStack?.Contains(overlayCam) ?? false)
                            {
                                baseCam.cameraStack.Add(overlayCam);
                            }
                        }
                    }
                }
            }
        }

        private void Start()
        {
            _camera = GetComponent<UCamera>();
            _cameraData = GetComponent<UniversalAdditionalCameraData>();

            if (_cameraData.renderType == CameraRenderType.Overlay)
            {
                if (!_overlayCameras.TryGetValue(baseCameraId, out var cameras))
                {
                    cameras = new HashSet<UCamera>();
                    _overlayCameras.Add(baseCameraId, cameras);
                }

                cameras.Add(_camera);
            }
            else
            {
                if (!_baseCameras.TryGetValue(cameraId, out var cameras))
                {
                    cameras = new HashSet<UniversalAdditionalCameraData>();
                    _baseCameras.Add(baseCameraId, cameras);
                }

                cameras.Add(_cameraData);
            }

            RefreshStacks();
        }

        private void OnDestroy()
        {
            if (!_camera)
            {
                return;
            }

            if (_cameraData.renderType == CameraRenderType.Overlay)
            {
                if (_overlayCameras.TryGetValue(baseCameraId, out var cameras))
                {
                    cameras.Remove(_camera);
                }
            }
            else
            {
                if (_baseCameras.TryGetValue(cameraId, out var cameras))
                {
                    cameras.Remove(_cameraData);
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CameraOverlayController))]
    class CameraOverlayControllerEditor : UnityEditor.Editor
    {
        private CameraOverlayController _target;
        private UniversalAdditionalCameraData _cameraData;
        private SerializedProperty _cameraIdProp;
        private SerializedProperty _baseCameraIdProp;


        private void OnEnable()
        {
            _target = (CameraOverlayController)target;
            _cameraData = _target.GetComponent<UniversalAdditionalCameraData>();
            _cameraIdProp = serializedObject.FindProperty(nameof(CameraOverlayController.cameraId));
            _baseCameraIdProp = serializedObject.FindProperty(nameof(CameraOverlayController.baseCameraId));
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            if (_cameraData.renderType == CameraRenderType.Overlay)
            {
                EditorGUILayout.PropertyField(_baseCameraIdProp);
            }
            else
            {
                EditorGUILayout.PropertyField(_cameraIdProp);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
#endif
}
#endif