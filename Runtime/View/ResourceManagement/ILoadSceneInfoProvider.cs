using System.Numerics;

namespace GBG.GameToolkit.View.ResourceManagement
{
    /// <summary>
    /// Provides information to determine which areas should be loaded.
    /// </summary>
    public interface ILoadSceneInfoProvider
    {
        Vector3 GetCentralPosition();
        float GetPreloadDistance();
        float GetUnloadDistance();
    }
}
