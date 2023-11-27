namespace GBG.Framework.Logic
{
    public interface IComponent
    {
        void Initialize();
        void LateInitialize();
        void Destroy();
    }
}
