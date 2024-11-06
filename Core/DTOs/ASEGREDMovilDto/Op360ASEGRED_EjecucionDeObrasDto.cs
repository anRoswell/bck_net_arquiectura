using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDMovilDto
{
    public class Op360ASEGRED_EjecucionDeObrasDto
    {
        public string codigo_obra { get; set; }
        public string nombre_obra { get; set; }
        public string numero_pet { get; set; }
        public string numero_ota { get; set; }
        public string tipo { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_cierre { get; set; }
    }
}
