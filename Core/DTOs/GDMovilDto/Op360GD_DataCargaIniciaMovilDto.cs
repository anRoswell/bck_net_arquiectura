using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360GD_DataCargaIniciaMovilDto
    {
        public Pregunta[] preguntas { get; set; }
        public Causales_Incidencia[] causales_incidencia { get; set; }
        public Tipo_Cierre[] tipo_cierre { get; set; }
        public Modificaciones_Trafo[] modificaciones_trafo { get; set; }
        public Accione[] acciones { get; set; }
        public Rechazo_Orden[] rechazo_orden { get; set; }
        public Regla_De_Oro[] regla_de_oro { get; set; }
    }

    public class Pregunta
    {
        public int id_pregunta { get; set; }
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public string ind_activo { get; set; }
    }

    public class Causales_Incidencia
    {
        public int id_causa_incidencia { get; set; }
        public string descripcion { get; set; }
        public string ind_activo { get; set; }
    }

    public class Tipo_Cierre
    {
        public int id_cierre_tipo { get; set; }
        public string descripcion { get; set; }
        public string ind_activo { get; set; }
    }

    public class Modificaciones_Trafo
    {
        public int id_modificacion_transformador { get; set; }
        public string descripcion { get; set; }
        public string ind_activo { get; set; }
    }

    public class Accione
    {
        public int id_accion { get; set; }
        public string descripcion { get; set; }
        public string ind_activo { get; set; }
    }

    public class Rechazo_Orden
    {
        public int id_rechazo_motivo { get; set; }
        public string descripcion { get; set; }
        public string ind_activo { get; set; }
    }

    public class Regla_De_Oro
    {
        public int id_regla_oro { get; set; }
        public string descripcion { get; set; }
        public string ind_activo { get; set; }
    }

}
