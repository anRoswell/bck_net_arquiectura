using Core.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryActaLiquidacion
    {
        public IFormFile File { get; set; }
        public string Contrato { get; set; }
    }

    public class ContratoActaLiquidacion : BaseQuery
    {
        public int IdContrato { get; set; }
        public int? CodArchivoActaLiquidacion { get; set; }
        public int? KeyFileArchivoActaLiquidacion { get; set; }
        public string NombreArchivoActaLiquidacion { get; set; }
        public string UrlArchivoActaLiquidacion { get; set; }
        public DocReqUploadDto DocumentoAGuardar { get; set; }
    }
}
