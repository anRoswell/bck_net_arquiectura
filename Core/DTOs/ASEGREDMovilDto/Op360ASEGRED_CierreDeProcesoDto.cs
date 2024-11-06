using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDMovilDto
{
    public class Op360ASEGRED_CierreDeProcesoDto
    {
        public string Firma_tecnico_base64 { get; set; }
        public List<string> Fotografias_base64 { get; set; }
    }

    /* Ejemplo del JSON
    {
      "Firma_tecnico_base64": "cadenadebase64parafirma",
      "Fotografias_base64": ["foto1", "foto2", "foto3"]
    }*/
}
