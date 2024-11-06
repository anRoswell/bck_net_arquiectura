using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.GDWebEntities
{
    public class Op360GD_Orden_Agrupada_Response
    {
        public GD_Orden_Agrupada[] ordenes_agrupadas {  get; set; }
        public Grafica_Asignacion[] grafica_asignacion { get; set; }
    }

    public class Grafica_Asignacion
    {
        public string asignacion { get; set; }
        public int noregistros { get; set; }
    }

    public class GD_Orden_Agrupada
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
