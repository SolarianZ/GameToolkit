using System.Numerics;

namespace GBG.Framework.View.SceneManagement
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
