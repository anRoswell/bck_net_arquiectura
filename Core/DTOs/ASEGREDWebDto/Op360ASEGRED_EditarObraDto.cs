using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_EditarObraDto
    {
        public string nombre { get; set; }
        public string objetivo { get; set; }
        public string alcance { get; set; }
        public int presupuesto { get; set; }
        public int costo_mensual_interventoria { get; set; }
        public int costo_mensual_aire { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public int id_interventor { get; set; }
        public string id_contratista { get; set; }
    }
}
