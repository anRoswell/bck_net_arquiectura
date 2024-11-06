using Core.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryPolizasRenovadas
    {
        public List<IFormFile> Files { get; set;}
        public string Contrato { get; set; }
    }

    public class QueryContratoPolizasRenovadas : BaseQuery
    {
        public int? IdContrato { get; set; }
        public List<DocReqPolizaDto> DocumentosReqPoliza { get; set; }
        public List<DocReqUploadDto> DocumentosAguardar { get; set; }
    }
}
