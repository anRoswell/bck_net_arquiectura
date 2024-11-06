using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360_Cierre_OrdenDto
    {
        public int id_tipo_cierre { get; set; }
        public string observacion { get; set; }
        public int id_modificacion_tranformador { get; set; }
        public string hora_apertura { get; set; }
        public string hora_cierre { get; set; }
    }
}
