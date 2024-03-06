using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.ConfigData
{
    public interface IConfigTablePtr
    {
        Type GetConfigType();
    }

    public interface IConfigTable<T> : IConfigTablePtr where T : IConfig
    {
        bool ContainsConfig(int key);
        IReadOnlyList<T> GetConfigs();
        T GetConfig(int key);
        bool TryGetConfig(int key, out T value);
    }
}
