using Core.CustomEntities;
using Newtonsoft.Json;
using System;

namespace Api.Responses
{
    public class ApiResponseUrlPlantilla<T>
    {
        public int Status { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }
        public Metadata2 Meta { get; set; }
        public int TotalRecords { get; set; }        

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url_plantilla_Generacion_Ordenes { get; set; }
        public Uri Url_plantilla_Reasignacion_Contratistas { get; set; }
        public Uri Url_plantilla_Legalizacion_Orden { get; set; }
        public Uri Url_plantilla_Asignacion_Tecnico { get; set; }
        public Uri Url_plantilla_DesAsignacion_Tecnico { get; set; }
        public Uri Url_plantilla_Reasignacion_Contratistas2 { get; set; }

        public ApiResponseUrlPlantilla(T data, int status)
        {
            Data = data;
            Status = status;
            Mensaje = "Registros Creados con Éxito";
        }

        public ApiResponseUrlPlantilla(T data, int status, int totalRecords)
        {
            Data = data;
            Status = status;
            TotalRecords = totalRecords;
        }

        public ApiResponseUrlPlantilla(T data, int status, string mensaje)
        {
            Data = data;
            Status = status;
            Mensaje = mensaje;
        }
    }    
}