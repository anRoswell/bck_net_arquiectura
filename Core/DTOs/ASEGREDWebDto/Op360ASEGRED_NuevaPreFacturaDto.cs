using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_NuevaPreFacturaDto
    {
        public string razon_social { get; set; }
        public int nit { get; set; }
        public string direccion { get; set; }
        public int telefono { get; set; }
        public int num_contrato { get; set; }
        public string fecha { get; set; }
        public string plazo_dias { get; set; }
        public string subestacion { get; set; }
        public string dirigido { get; set; }
        public string observaciones { get; set; }
    }
}
