using System;
using System.Collections.Generic;

namespace GBG.Framework.ConfigData
{
    public interface IConfigTablePtr
    {
        Type GetConfigType();
    }

    public interface IConfigTable<T> : IConfigTablePtr where T : IConfig
    {
        bool ContainsConfig(int key);
        IReadOnlyList<T> GetConfigs();
        T GetConfig(int key, T defaultValue = default);
        bool TryGetConfig(int key, out T value);
    }
}
