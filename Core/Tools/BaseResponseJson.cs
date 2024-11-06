using Core.Exceptions;
using Core.ModelResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Tools
{
    public static class BaseResponseJson<T>
    {
        public static T ConvertJsonToEntity(List<ResponseJsonString> dataJson)
        {
            try
            {
                string json = dataJson?.FirstOrDefault()?.JsonResponse;
                if (json is null) return default;
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static T ConvertJsonToEntity(object dataJson)
        {
            try
            {
                string json = dataJson.ToString();
                if (json is null)
                    return default;
                T participantes = JsonConvert.DeserializeObject<T>(json);
                return participantes;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
