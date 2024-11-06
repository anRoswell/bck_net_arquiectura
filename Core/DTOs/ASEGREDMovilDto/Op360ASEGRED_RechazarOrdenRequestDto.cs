using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDMovilDto
{
    public class Op360ASEGRED_RechazarOrdenRequestDto
    {
        public int id_orden { get; set; }
        public int id_contratista_persona { get; set; }
        public DateTime fecha_rechazo { get; set; }
        public string observacion_rechazo { get; set; }
    }
}
