using System;

namespace Core.DTOs
{
    public class DocReqProveedorOtroDto
    {
        public int? Id { get; set; }
        public int DrpoCodContrato { get; set; }
        public int DrpoCodDocumento { get; set; }
        public string DrpoNombreDocumento { get; set; }
        public int DrpoVigencia { get; set; }
        public bool? DrpoObligatorio { get; set; }
        public int? CodArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public string UrlArchivo { get; set; }
        public int KeyFile { get; set; }
    }
}
