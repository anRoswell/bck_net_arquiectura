using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_CrearSoporteDto
    {
        public int item { get; set; }
        public string tipo_soporte { get; set; }
        public string nombre_soporte { get; set; }
        public string usuario_carga { get; set; }
        public string estado { get; set; }
        public string validacion { get; set; }
    }
}
