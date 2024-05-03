﻿using System;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity.Editor.AssetChecker
{
    public enum ResultType
    {
        NotImportant,
        Warning,
        Error,
    }

    [Serializable]
    public class AssetCheckResult
    {
        public ResultType type;
        public string title;
        public string details;
        public UObject asset;
        public AssetChecker checker;
        public bool repairable;
    }
}