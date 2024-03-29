﻿using System;
using UnityEngine;

namespace GBG.GameToolkit.Unity
{
    public abstract class SceneAreaTrigger : MonoBehaviour
    {
        public static ulong EnterFunctions { get; set; } = ulong.MaxValue;
        public static ulong ExitFunctions { get; set; } = ulong.MaxValue;

        public static event Handler OnTriggered;
        public static event Handler2D OnTriggered2D;
        public static event Handler3D OnTriggered3D;


        public abstract AreaParams GetAreaParams();
        public abstract ulong GetAreaFunctions();


        // Expose to derived classes for debugging purposes.
        protected static void Trigger(ulong functions, AreaParams areaParams, Component instigatorCollider, bool isExitArea)
        {
            OnTriggered?.Invoke(functions, areaParams, instigatorCollider, isExitArea);
        }

        // Expose to derived classes for debugging purposes.
        protected static void Trigger2D(ulong functions, AreaParams areaParams, Collider2D instigator, bool isExitArea)
        {
            OnTriggered2D?.Invoke(functions, areaParams, instigator, isExitArea);
            OnTriggered?.Invoke(functions, areaParams, instigator, isExitArea);
        }

        // Expose to derived classes for debugging purposes.
        protected static void Trigger3D(ulong functions, AreaParams areaParams, Collider instigator, bool isExitArea)
        {
            OnTriggered3D?.Invoke(functions, areaParams, instigator, isExitArea);
            OnTriggered?.Invoke(functions, areaParams, instigator, isExitArea);
        }


        #region Unity Messages

        // Expose to derived classes for debugging purposes.
        protected void OnTriggerEnter(Collider instigator)
        {
            ulong areaFunctions = GetAreaFunctions();
            if ((areaFunctions & EnterFunctions) == 0)
            {
                return;
            }

            Trigger3D(areaFunctions, GetAreaParams(), instigator, false);
        }

        // Expose to derived classes for debugging purposes.
        protected void OnTriggerExit(Collider instigator)
        {
            ulong areaFunctions = GetAreaFunctions();
            if ((areaFunctions & ExitFunctions) == 0)
            {
                return;
            }

            Trigger3D(areaFunctions, GetAreaParams(), instigator, true);
        }

        // Expose to derived classes for debugging purposes.
        protected void OnTriggerEnter2D(Collider2D instigator)
        {
            ulong areaFunctions = GetAreaFunctions();
            if ((areaFunctions & EnterFunctions) == 0)
            {
                return;
            }

            Trigger2D(areaFunctions, GetAreaParams(), instigator, false);
        }

        // Expose to derived classes for debugging purposes.
        protected void OnTriggerExit2D(Collider2D instigator)
        {
            ulong areaFunctions = GetAreaFunctions();
            if ((areaFunctions & ExitFunctions) == 0)
            {
                return;
            }

            Trigger2D(areaFunctions, GetAreaParams(), instigator, true);
        }

        #endregion


        public delegate void Handler(ulong functions, AreaParams areaParams, Component instigatorCollider, bool isExit);
        public delegate void Handler2D(ulong functions, AreaParams areaParams, Collider2D instigator, bool isExit);
        public delegate void Handler3D(ulong functions, AreaParams areaParams, Collider instigator, bool isExit);

        [Serializable]
        public abstract class AreaParams
        {
        }
    }
}