using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    /// <summary>
    /// clave: aaaaORDENESa65sd4f65sdf _01b
    /// </summary>
    public class Aire_Scr_Ord_OrdenDto
    {
        public int id_orden { get; set; }
        public int id_tipo_orden { get; set; }
        public string numero_orden { get; set; }
        public int id_estado_orden { get; set; }
        public int id_contratista { get; set; }
        public int id_cliente { get; set; }
        public int id_territorial { get; set; }
        public int id_zona { get; set; }

        public string direcion { get; set; }

        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public string id_usuario_cierre { get; set; }
        public string descripcion { get; set; }
        public string comentarios { get; set; }
        public string acta { get; set; }
        public int id_actividad { get; set; }
        public int id_contratista_persona { get; set; }
    }
}
