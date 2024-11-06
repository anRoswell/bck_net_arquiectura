using Core.CustomEntities;
using Newtonsoft.Json;
using System;

namespace Api.Responses
{
    public class ApiResponseCargueMasivo<T>
    {
        public int Status { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }
        public MedirTiempo Meta { get; set; }
        public int TotalRecords { get; set; }

        public ApiResponseCargueMasivo(string mensaje, int status)
        {
            Status = status;
            Mensaje = mensaje;
        }

        public ApiResponseCargueMasivo(T data, int status, string mensaje, int totalRecords)
        {
            Data = data;
            Status = status;
            Mensaje = mensaje;
            TotalRecords = totalRecords;
        }        
    }

    public class MedirTiempo
    {
        public string GuardarArchivo { get; set; }
        public string CargueDataTemporal { get; set; }
        public string Procesamiento { get; set; }
        public string TiempoTotal { get; set; }
        public int id_soporte { get; set; }
    }
}