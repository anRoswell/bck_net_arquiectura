using Core.DTOs;
using Microsoft.AspNetCore.Http;
using System;

namespace Core.QueryFilters
{
    public class QueryNotificacionNoProrroga : BaseQuery
    {
        public IFormFile File { get; set; }
        public string Contrato { get; set; }
    }

    public class ContratoNotificacionNoProrroga : BaseQuery
    { 
        public int IdContrato { get; set; }
        public int? CodArchivoNotificacionNoProrroga { get; set; }
        public int? KeyFileArchivoNotificacionNoProrroga { get; set; }
        public string NombreArchivoNotificacionNoProrroga { get; set; }
        public string UrlArchivoNotificacionNoProrroga { get; set; }
        public DocReqUploadDto DocumentoAGuardar { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public string Observacion { get; set; }
    }
}
