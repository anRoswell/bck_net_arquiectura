using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_ListarDetalleEstructuraDto
    {
        public int item { get; set; }
        public string nombre_poste { get; set; }
        public string georeferencia { get; set; }
        public string estado { get; set; }
        public string fecha_ultimo_trabajo { get; set; }
        public int numero_de_trabajos { get; set; }
        public string fecha_ultima_auditoria { get; set; }
        public int numero_de_auditorias { get; set; }
    }
}
