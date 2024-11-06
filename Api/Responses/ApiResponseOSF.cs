using Core.CustomEntities;
using Newtonsoft.Json;
using System;

namespace Api.Responses
{
    public class ApiResponseOSF<T>
    {
        public int Status { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }

        public ApiResponseOSF(T data, int status, string mensaje)
        {
            Status = status;
            Mensaje = mensaje;
            Data = data;
        }        
    }

    public class TokenOSF
    {
        public string JwtTokenOSF { get; set; }
    }
}