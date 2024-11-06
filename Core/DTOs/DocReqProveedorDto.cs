using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class DocReqProveedorDto
    {
        public int? Id { get; set; }
        public int DrpCodContrato { get; set; }
        public int DrpCodDocumento { get; set; }
        public int? DrpCodPrvDocumento { get; set; }
        public bool? DrpAprobado { get; set; }
        public bool? DrpObligatorio { get; set; }
        public int? DrpTipoVersion { get; set; }
        public int? CodArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public string UrlArchivo { get; set; }
        public int KeyFile { get; set; }
        public string PrvdNameDocument          { get; set; }
        public string PrvdOriginalNameDocument  { get; set; }
        public string PrvdExtDocument           { get; set; }
        public int PrvdSizeDocument          { get; set; }
        public string PrvdUrlDocument           { get; set; }
        public string PrvdUrlRelDocument        { get; set; }
    }
}
