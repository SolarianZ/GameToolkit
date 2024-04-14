using System;

namespace GBG.GameToolkit.AI.Parameter
{
    public interface IParamInstance : IParamValueProvider, IUniqueItem
    {
        static float Epsilon = 1E-5f;

        string Name { get; }

        event Action<IParamInstance> OnValueChanged;


        #region Setter

        void SetFloat32(float value);
        void SetInt32(int value);
        void SetBool(bool value);
        void SetVector32(Vector32 value);
        void SetString(string value);
        void SetObject(object value);

        #endregion
    }
}