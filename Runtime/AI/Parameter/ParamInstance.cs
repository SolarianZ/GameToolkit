using System;

namespace GBG.GameToolkit.AI.Parameter
{
    public class ParamInstance : IParamInstance
    {
        string IUniqueItem.Guid => Guid;

        public string Guid { get; }
        public string Name { get; }
        public ParamType Type { get; }

        internal Vector32 Vector32Value;
        internal int Int32Value;
        internal object ObjectValue;

        public event Action<IParamInstance> OnValueChanged;


        public ParamInstance(IParamData data)
        {
            Name = data.Name;
            Guid = data.Guid;
            Type = data.Literal.Type;
            Vector32Value = data.Literal.GetVector32();
            Int32Value = data.Literal.GetInt32();
            ObjectValue = data.Literal.GetObject();
        }


        #region Getter

        public float GetFloat32()
        {
            return Vector32Value.X;
        }

        public int GetInt32()
        {
            return Int32Value;
        }

        public bool GetBool()
        {
            return Int32Value != 0;
        }

        public Vector32 GetVector32()
        {
            return Vector32Value;
        }

        public string GetString()
        {
            return (string)ObjectValue;
        }

        public object GetObject()
        {
            return ObjectValue;
        }

        public bool TryGetObject<T>(out T value)
        {
            if (ObjectValue is T t)
            {
                value = t;
                return true;
            }

            value = default;
            return false;
        }

        #endregion


        #region Setter

        public void SetFloat32(float value)
        {
            if (Math.Abs(Vector32Value.X - value) < IParamInstance.Epsilon)
            {
                return;
            }

            Vector32Value.X = value;
            OnValueChanged?.Invoke(this);
        }

        public void SetInt32(int value)
        {
            if (Int32Value == value)
            {
                return;
            }

            Int32Value = value;
            OnValueChanged?.Invoke(this);
        }

        public void SetBool(bool value)
        {
            if ((Int32Value != 0) == value)
            {
                return;
            }

            Int32Value = value ? 1 : 0;
            OnValueChanged?.Invoke(this);
        }

        public void SetVector32(Vector32 value)
        {
            if (Math.Abs(Vector32Value.X - value.X) < IParamInstance.Epsilon &&
                Math.Abs(Vector32Value.Y - value.Y) < IParamInstance.Epsilon &&
                Math.Abs(Vector32Value.Z - value.Z) < IParamInstance.Epsilon &&
                Math.Abs(Vector32Value.W - value.W) < IParamInstance.Epsilon)
            {
                return;
            }

            Vector32Value = value;
            OnValueChanged?.Invoke(this);
        }

        public void SetString(string value)
        {
#pragma warning disable CS0252 // 可能非有意的引用比较；左侧需要强制转换
            if (ObjectValue == value)
            {
                return;
            }
#pragma warning restore CS0252 // 可能非有意的引用比较；左侧需要强制转换

            ObjectValue = value;
            OnValueChanged?.Invoke(this);
        }

        public void SetObject(object value)
        {
            if (ObjectValue == value)
            {
                return;
            }

            ObjectValue = value;
            OnValueChanged?.Invoke(this);
        }

        #endregion
    }
}