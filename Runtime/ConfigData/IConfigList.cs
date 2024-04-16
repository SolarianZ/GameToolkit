using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.ConfigData
{
    public interface IConfigListPtr
    {
        Type GetConfigType();
    }

    public interface IConfigList<T> : IConfigListPtr where T : IConfig
    {
        bool ContainsConfig(int key);
        IReadOnlyList<T> GetConfigs();
        T GetConfig(int key);
        bool TryGetConfig(int key, out T value);
    }
}
