namespace GBG.Framework.Logic
{
    public interface IComponent
    {
        void Initialize();
        void PostInitialize();
        void Destroy();
    }
}
