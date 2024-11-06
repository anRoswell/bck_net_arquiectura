using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryRechazarDocumentosProveedor : BaseQuery
    {
        public int ContIdContrato { get; set; }
        public List<DocReqProveedorDto> DocumentosPrv { get; set; }
        public string ContJustificacionRechazo { get; set; }
        public string DocumentosPrvEliminados { get; set; }
        public List<DocReqUploadDto> DocumentosAguardar { get; set; }
    }
}
