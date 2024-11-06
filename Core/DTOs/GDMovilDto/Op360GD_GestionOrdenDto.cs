using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360GD_GestionOrdenDto
    {
        public Op360_PreguntaDto pregunta { get; set; }
        public Op360_CausaDto causa { get; set; }
        public IList<Op360_Reglas_De_OroDto> reglas_de_oro { get; set; }
        public IList<Op360_Materiales_InstalarDto> materiales_instalar { get; set; }
        public IList<Op360_Materiales_RetirarDto> materiales_retirar { get; set; }
        public Op360_Cierre_OrdenDto cierre_orden { get; set; }
        public IList<int> acciones { get; set; }
        public IList<Op360_SoporteDTO> soportes { get; set; }
    }

}
