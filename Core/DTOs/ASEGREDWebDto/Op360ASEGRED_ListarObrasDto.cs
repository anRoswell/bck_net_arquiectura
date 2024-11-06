using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_ListarObrasDto
    {
        public int id_centro_costo { get; set; }
        public int id_centro_responsable { get; set; }
        public int id_proyecto_tipo { get; set; }
        public int id_obra_tipo { get; set; }
        public string nombre { get; set; }
        public string objetivo { get; set; }
        public string alcance { get; set; }
        public int presupuesto { get; set; }
        public int costo_mensual_interventoria { get; set; }
        public int costo_mensual_aire { get; set; }
        public int id_plan { get; set; }
        public int id_pep { get; set; }
        public int id_orden_trabajo_mano_obra { get; set; }
        public int id_orden_trabajo_materiales { get; set; }
        public string id_municipio { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public int id_interventor { get; set; }
        public string id_contratista { get; set; }
        public int? id_estado { get; set; }
        public int id_usuario { get; set; }
    }
}
