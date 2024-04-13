namespace GBG.GameToolkit.AI.Parameter
{
    public interface IParamValueProvider
    {
        ParamType Type { get; }


        float GetFloat32();
        int GetInt32();
        bool GetBool();
        Vector32 GetVector32();
        string GetString();
        object GetObject();
        bool TryGetObject<T>(out T value);
    }
}