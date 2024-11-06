using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Op360Ordenes_TrazabilidadDto
    {
        public int id_orden { get; set; }
        public Traza[] traza { get; set; }
    }

    public class Traza
    {
        public int id { get; set; }
        public int id_estado_orden { get; set; }
        public string icono_Esado { get; set; }
        public string nombre_estado_orden { get; set; }
        public int? id_contratista { get; set; }
        public string nombre_contratista { get; set; }
        public int? id_contratista_persona { get; set; }
        public string nombre_contratista_persona { get; set; }
        public string observacion { get; set; }
        public string fecha { get; set; }
    }
}
