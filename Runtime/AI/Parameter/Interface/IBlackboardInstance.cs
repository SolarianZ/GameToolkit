using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.AI.Parameter
{
    /// <summary>
    /// 黑板。
    /// </summary>
    public interface IBlackboardInstance
    {
        #region Param

        /// <summary>
        /// 参数名称-参数对象映射表。
        /// </summary>
        IReadOnlyDictionary<string, IParamInstance> ParamNameTable { get; }
        /// <summary>
        /// 参数GUID-参数对象映射表。
        /// </summary>
        IReadOnlyDictionary<string, IParamInstance> ParamGuidTable { get; }

        /// <summary>
        /// 参数值变化事件。
        /// </summary>
        event Action<IParamInstance> OnParamValueChanged;


        IParamInstance GetParamByName(string name)
        {
            return ParamNameTable.GetValueOrDefault(name);
        }

        IParamInstance GetParamByGuid(string guid)
        {
            if (ParamGuidTable.TryGetValue(guid, out IParamInstance param))
            {
                return param;
            }

            return null;
        }

        bool TrtGetFloat32ByName(string name, out float value)
        {
            IParamInstance param = GetParamByName(name);
            if (param == null || param.Type != ParamType.Float32)
            {
                value = default;
                return false;
            }

            value = param.GetFloat32();
            return true;
        }

        bool TryGetInt32ByName(string name, out int value)
        {
            IParamInstance param = GetParamByName(name);
            if (param == null || param.Type != ParamType.Int32)
            {
                value = default;
                return false;
            }

            value = param.GetInt32();
            return true;
        }

        bool TryGetBoolByName(string name, out bool value)
        {
            IParamInstance param = GetParamByName(name);
            if (param == null || param.Type != ParamType.Bool)
            {
                value = default;
                return false;
            }

            value = param.GetBool();
            return true;
        }

        bool TryGetVector32ByName(string name, out Vector32 value)
        {
            IParamInstance param = GetParamByName(name);
            if (param == null || param.Type != ParamType.Vector32)
            {
                value = default;
                return false;
            }

            value = param.GetVector32();
            return true;
        }

        bool TryGetStringByName(string name, out string value)
        {
            IParamInstance param = GetParamByName(name);
            if (param == null || param.Type != ParamType.String)
            {
                value = default;
                return false;
            }

            value = param.GetString();
            return true;
        }

        bool TryGetObjectByName<T>(string name, out T value)
        {
            IParamInstance param = GetParamByName(name);
            if (param == null || !param.Type.IsObjectType())
            {
                value = default;
                return false;
            }

            return param.TryGetObject(out value);
        }

        #endregion


        #region Custom Data

        /// <summary>
        /// 用户自定义数据表。
        /// </summary>
        internal Dictionary<object, object> CustomDataTable { get; set; }


        void SetCustomData(object key, object value)
        {
            CustomDataTable ??= new Dictionary<object, object>();
            CustomDataTable[key] = value;
        }

        object GetCustomData(object key)
        {
            if (CustomDataTable != null && CustomDataTable.TryGetValue(key, out object value))
            {
                return value;
            }

            return null;
        }

        bool TryGetCustomData<T>(object key, out T value)
        {
            if (GetCustomData(key) is T t)
            {
                value = t;
                return true;
            }

            value = default;
            return false;
        }

        void RemoveCustomData(object key)
        {
            if (CustomDataTable != null)
            {
                CustomDataTable.Remove(key);
            }
        }

        void ClearCustomData()
        {
            CustomDataTable?.Clear();
        }

        #endregion
    }
}