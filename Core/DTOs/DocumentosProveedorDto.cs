using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class DocumentosProveedorDto
    {
        public int IdContrato { get; set; }
        public List<DocReqProveedorDto> DocumentosPrv { get; set; }
        public List<DocReqProveedorOtroDto> DocumentosPrvOtros { get; set; }
        public List<DocReqUploadDto> DocumentosAguardar { get; set; }
        public string DocumentosPrvEliminados { get; set; }
        public string DocumentosPrvOtrosEliminados { get; set; }

        public string CodUser { get; set; }
        public string CodUserUpdate { get; set; }
    }
}
