using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_ListarHistorialPreFacturasParcialesDto
    {
        public int item { get; set; }
        public string nombre_obra { get; set; }
        public int codigo { get; set; }
        public string numero_contrato { get; set; }
        public string fecha { get; set; }
        public string plazo { get; set; }
        public string circuito { get; set; }
        public string subestacion { get; set; }
        public string dirigido { get; set; }

        public Op360ASEGRED_ListarHistorialPreFacturasParcialesDto()
        {
            item = 0;
        }
    }
}
