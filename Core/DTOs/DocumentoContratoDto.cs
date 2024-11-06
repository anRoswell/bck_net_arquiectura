using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class DocumentoContratoDto
    {
        public int? Id { get; set; }
        public int CodContrato { get; set; }
        public int CodTipoDocumento { get; set; }
        public int? CodDocumento { get; set; }
        public string DcPermisos { get; set; }
        public int? CodArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public string UrlArchivo { get; set; }
        public int KeyFile { get; set; }
    }
}
