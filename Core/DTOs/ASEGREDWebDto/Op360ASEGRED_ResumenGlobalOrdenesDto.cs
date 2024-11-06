using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_ResumenGlobalOrdenesDto
    {
        public Resumen_Ordenes[] resumen_ordenes { get; set; }
    }

    public class Resumen_Ordenes
    {
        public int id_contratista { get; set; }
        public string nombre_contratista { get; set; }
        public int id_zona { get; set; }
        public string nombre_zona { get; set; }
        public int id_estado_orden { get; set; }
        public string nombre_estado_orden { get; set; }
        public int NoRegistros { get; set; }
    }
}
