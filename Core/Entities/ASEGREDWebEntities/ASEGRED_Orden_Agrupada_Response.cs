using Core.Entities.GDWebEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.ASEGREDWebEntities
{
    public class ASEGRED_Orden_Agrupada_Response
    {
        public ASEGRED_Orden_Agrupada[] ordenes_agrupadas { get; set; }
        public Grafica_Asignacion[] grafica_asignacion { get; set; }
    }

    public class Grafica_Asignacion
    {
        public string asignacion { get; set; }
        public int noregistros { get; set; }
    }

    public class ASEGRED_Orden_Agrupada
    {
        public int id_contratista { get; set; }
        public string contratista { get; set; }
        public string identificacion { get; set; }
        public Zona_Response[] zonas { get; set; }
        public int[] zonas_array
        {
            get
            {
                return zonas.Select(x => x.id_zona).ToArray();
            }
        }
        public int NoRegistros { get; set; }
    }
}
