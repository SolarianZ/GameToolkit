using System;
using System.Collections.Generic;

namespace GBG.GameToolkit.AI.Parameter
{
    /// <summary>
    /// 黑板。
    /// </summary>
    public class BlackboardInstance : IBlackboardInstance
    {
        public BlackboardInstance(IBlackboardData data)
        {
            // Param
            _paramNameTable = new Dictionary<string, IParamInstance>(data.Params.Count);
            _paramGuidTable = new Dictionary<string, IParamInstance>(data.Params.Count);
            foreach (IParamData paramData in data.Params)
            {
                IParamInstance param = new ParamInstance(paramData);
                param.OnValueChanged += OnParamValueChangedInternal;
                _paramNameTable.Add(param.Name, param);
                _paramGuidTable.Add(param.Guid, param);
            }
        }


        #region Param

        /// <inheritdoc />
        public IReadOnlyDictionary<string, IParamInstance> ParamNameTable => _paramNameTable;
        private readonly Dictionary<string, IParamInstance> _paramNameTable;
        /// <inheritdoc />
        public IReadOnlyDictionary<string, IParamInstance> ParamGuidTable => _paramGuidTable;
        private readonly Dictionary<string, IParamInstance> _paramGuidTable;

        /// <inheritdoc />
        public event Action<IParamInstance> OnParamValueChanged;


        private void OnParamValueChangedInternal(IParamInstance instance)
        {
            OnParamValueChanged?.Invoke(instance);
        }

        #endregion


        #region Custom Data

        /// <inheritdoc />
        Dictionary<object, object> IBlackboardInstance.CustomDataTable { get; set; }

        #endregion
    }
}