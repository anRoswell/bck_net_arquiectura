using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDMovilDto
{
    public class Op360ASEGRED_ListarInspeccionesDto
    {
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string tipo { get; set; }
        public string estado { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string responsable { get; set; }
        public string info { get; set; }
    }
}
