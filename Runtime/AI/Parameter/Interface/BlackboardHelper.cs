using System;

namespace GBG.GameToolkit.AI.Parameter
{
    public static class BlackboardHelper
    {
        /// <summary>
        /// 按GUID获取参数，当参数不存在时抛出异常。
        /// </summary>
        /// <param name="blackboard"></param>
        /// <param name="paramGuid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IParamInstance GetParamByGuidWithException(this IBlackboardInstance blackboard, string paramGuid)
        {
            IParamInstance param = blackboard.GetParamByGuid(paramGuid);
            if (param == null)
            {
                throw new ArgumentException($"Param not found. Param guid: {paramGuid}.");
            }

            return param;
        }

        /// <summary>
        /// 按GUID获取参数，当参数不存在或参数类型不匹配时抛出异常。
        /// </summary>
        /// <param name="blackboard"></param>
        /// <param name="paramGuid"></param>
        /// <param name="paramType"></param>
        /// <param name="alternativeParamType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IParamInstance GetParamByGuidWithException(this IBlackboardInstance blackboard, string paramGuid, ParamType paramType, ParamType? alternativeParamType = null)
        {
            IParamInstance param = blackboard.GetParamByGuid(paramGuid);
            if (param == null)
            {
                throw new ArgumentException($"Param not found. Param guid: {paramGuid}.");
            }

            if (param.Type != paramType)
            {
                if (alternativeParamType == null || param.Type != alternativeParamType.Value)
                {
                    throw new ArgumentException($"Param type mismatch. Expected: {paramType}, actual: {param.Type}, param guid: {paramGuid}.");
                }
            }

            return param;
        }
    }
}