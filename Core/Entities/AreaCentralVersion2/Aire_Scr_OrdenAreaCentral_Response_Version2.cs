using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.AreaCentralVersion2
{
    public class Aire_Scr_OrdenAreaCentral_Response_Version2
    {
        public Aire_Scr_OrdenAreaCentral_Version2[] ordenes { get; set; }
        public Grafica_Asignacion_Version2[] grafica_asignacion { get; set; }
        public int[] ArrayIdOrdenesFiltradas { get; set; }
        //public int[] ArrayIdOrdenesFiltradas
        //{
        //    get
        //    {
        //        return ordenes.Select(x => x.id_orden).ToArray();
        //    }
        //}
        public int? RegistrosTotales { get; set; }
    }

    public class Aire_Scr_OrdenAreaCentral_Version2
    {
        [Key]
        public int id_orden { get; set; }
        public int id_tipo_orden { get; set; }
        public string tipo_orden { get; set; }

        public string numero_orden { get; set; }
        //public int id_estado_orden { get; set; }
        public string estado_orden { get; set; }

        //public int? id_contratista { get; set; }
        public string contratista { get; set; }

        //public int id_cliente { get; set; }
        public string cliente { get; set; }

        //public int? id_territorial { get; set; }
        public string territorial { get; set; }

        //public int? id_zona { get; set; }
        public string zona { get; set; }

        public string direcion { get; set; }

        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_cierre { get; set; }

        //public string id_usuario_cierre { get; set; }
        public string usuario_cierre { get; set; }

        //public int id_actividad { get; set; }
        public string actividad { get; set; }

        //public int? id_contratista_persona { get; set; }
        public string nombre_contratista_persona { get; set; }

        //public int? id_tipo_trabajo { get; set; }
        public string tipo_trabajo { get; set; }

        //public int? id_tipo_suspencion { get; set; }
        public string tipo_suspencion { get; set; }

        public string origen { get; set; }
        //public string mensaje_error_ws { get; set; }
    }

    public class Grafica_Asignacion_Version2
    {
        public string asignacion { get; set; }
        public int noregistros { get; set; }
    }

}
