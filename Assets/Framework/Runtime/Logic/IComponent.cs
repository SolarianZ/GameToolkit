namespace GBG.Framework.Logic
{
    public interface IComponent
    {
        int Priority { get; }


        void Initialize();
        void PostInitialize();
        void Destroy();
    }
}
