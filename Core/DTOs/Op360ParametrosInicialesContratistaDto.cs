using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Op360ParametrosInicialesContratistaDto
    {
        public int id_contratista { get; set; }
        public string identificacion_contratista { get; set; }
        public string nombre_contratista { get; set; }

        public Op360ParametrosInicialesContratista_Brigada[] Brigadas { get; set; }
        public Op360ParametrosInicialesContratista_Gnl_Zonas[] Zonas { get; set; }
        public Op360ParametrosInicialesContratista_Ord_Estados_Orden[] estados_orden { get; set; }
        public Op360ParametrosInicialesContratista_tipos_suspencion[] tipos_suspencion { get; set; }
        public Op360ParametrosInicialesContratista_Grafica_Asignacion[] grafica_asignacion { get; set; }

        public Uri Url_plantilla_Asignacion_Tecnico { get; set; }
        public Uri Url_plantilla_DesAsignacion_Tecnico { get; set; }

    }

    public class Op360ParametrosInicialesContratista_Brigada
    {
        public int id_contratista_brigada { get; set; }
        public int id_contratista_persona { get; set; }
        public string identificacion_contratista_persona { get; set; }
        public string nombre_contratista_persona { get; set; }
        public int id_contratista_vehiculo { get; set; }
        public string placa { get; set; }
        public int id_tipo_brigada { get; set; }
        public string codigo_tipo_brigada { get; set; }
        public string tipo_brigada { get; set; }
        public int[] zonas_brigada { get; set; }
    }

    public class Op360ParametrosInicialesContratista_Gnl_Zonas
    {
        [Key]
        public int id_zona { get; set; }
        public int id_territorial { get; set; }
        public int codigo { get; set; }
        public string nombre { get; set; }
    }

    public class Op360ParametrosInicialesContratista_Ord_Estados_Orden
    {
        [Key]
        public int id_estado_orden { get; set; }
        public string codigo_estado { get; set; }
        public string descripcion { get; set; }
    }

    public class Op360ParametrosInicialesContratista_tipos_suspencion
    {
        [Key]
        public int id_tipo_suspencion { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public Op360PrmIniCntrtst_tipos_brigada[] tipos_brigada { get; set; }
    }

    public class Op360PrmIniCntrtst_tipos_brigada
    {
        [Key]
        public int id_tipo_brigada { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }

    public class Op360ParametrosInicialesContratista_Grafica_Asignacion
    {
        public string asignacion { get; set; }
        public int noregistros { get; set; }
    }
}
