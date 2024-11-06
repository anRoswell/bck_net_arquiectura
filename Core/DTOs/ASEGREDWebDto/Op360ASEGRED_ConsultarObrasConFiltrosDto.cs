using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_ConsultarObrasConFiltrosDto
    {
        public string estado_obra { get; set; }
        public string tipo_obra { get; set; }
        public int codigo_obra { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
    }
}
