using Core.CustomEntities;
using Newtonsoft.Json;
using System;

namespace Api.Responses
{
    public class ApiResponseUrlPlantillaContratista<T>
    {
        public int Status { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }
        public Metadata2 Meta { get; set; }
        public int TotalRecords { get; set; }        

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url_plantilla_Asignacion_Tecnico { get; set; }
        public Uri Url_plantilla_DesAsignacion_Tecnico { get; set; }

        public ApiResponseUrlPlantillaContratista(T data, int status)
        {
            Data = data;
            Status = status;
            Mensaje = "Registros Creados con Éxito";
        }

        public ApiResponseUrlPlantillaContratista(T data, int status, int totalRecords)
        {
            Data = data;
            Status = status;
            TotalRecords = totalRecords;
        }

        public ApiResponseUrlPlantillaContratista(T data, int status, string mensaje)
        {
            Data = data;
            Status = status;
            Mensaje = mensaje;
        }
    }    
}