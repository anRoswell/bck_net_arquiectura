using Core.DTOs;
using Microsoft.AspNetCore.Http;
using System;

namespace Core.QueryFilters
{
    public class QueryNotificacionTerminacion: BaseQuery
    {
        public IFormFile File { get; set; }
        public string Contrato { get; set; }
    }

    public class ContratoNotificacionTerminacion : BaseQuery
    {
        public int IdContrato { get; set; }
        public int? CodArchivoNotificacionTerminacion { get; set; }
        public int? KeyFileArchivoNotificacionTerminacion { get; set; }
        public string NombreArchivoNotificacionTerminacion { get; set; }
        public string UrlArchivoNotificacionTerminacion { get; set; }
        public DocReqUploadDto DocumentoAGuardar { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public string Observacion { get; set; }
    }
}
