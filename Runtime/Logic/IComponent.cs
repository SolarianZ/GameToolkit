namespace GBG.GameToolkit.Logic
{
    public interface IComponent
    {
        void Initialize();
        void LateInitialize();
        void Destroy();
    }
}
