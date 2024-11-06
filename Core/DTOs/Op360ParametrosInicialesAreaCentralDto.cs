using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Op360ParametrosInicialesAreaCentralDto
    {
        public Op360ParametrosInicialesAreaCentral_Ord_Tipos_Orden[] tipos_orden { get; set; }
        public Op360ParametrosInicialesAreaCentral_Ord_Estados_Orden[] estados_orden { get; set; }
        public Op360ParametrosInicialesAreaCentral_V_Ctn_Contratistas[] contratistas { get; set; }
        public Op360ParametrosInicialesAreaCentral_Gnl_Territoriales[] territoriales { get; set; }
        public Op360ParametrosInicialesAreaCentral_Gnl_Zonas[] zonas { get; set; }
        public Op360ParametrosInicialesAreaCentral_Tipos_Suspencion[] tipos_suspencion { get; set; }
        
        public Op360ParametrosInicialesAreaCentral_Anomalias[] anomalias { get; set; }

        public Op360ParametrosInicialesAreaCentral_Estados_Servicio[] estados_servicio { get; set; }
        public Op360ParametrosInicialesAreaCentral_Parametros_Generales[] parametros_generales { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url_plantilla_Generacion_Ordenes { get; set; }
        public Uri Url_plantilla_Reasignacion_Contratista { get; set; }
        public Uri Url_plantilla_Legalizacion_Orden { get; set; }
        public Uri Url_plantilla_Asignacion_Tecnico { get; set; }
        public Uri Url_plantilla_DesAsignacion_Tecnico { get; set; }
        public Uri Url_plantilla_Reasignacion_Contratista2 { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Ord_Tipos_Orden
    {
        [Key]
        public int id_tipo_orden { get; set; }
        public string codigo_tipo_orden { get; set; }
        public string descripcion { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Ord_Estados_Orden
    {
        [Key]
        public int id_estado_orden { get; set; }
        public string codigo_estado { get; set; }
        public string descripcion { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_V_Ctn_Contratistas
    {
        [Key]
        public int id_contratista { get; set; }
        public int id_persona { get; set; }
        public string identificacion { get; set; }
        public string nombre_completo { get; set; }
        public string email { get; set; }
        public string ind_activo { get; set; }
        public string descripcion_ind_activo { get; set; }
        public string codigo { get; set; }
        public int[] zonas { get; set; }
        public Op360ParametrosInicialesAreaCentral_Brigada[] Brigadas { get; set; }

    }

    public class Op360ParametrosInicialesAreaCentral_Brigada
    {
        public int id_contratista_persona { get; set; }
        public string identificacion_contratista_persona { get; set; }
        public string nombre_contratista_persona { get; set; }
        public int id_tipo_brigada { get; set; }
        public int[] zonas_brigada { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Gnl_Territoriales
    {
        [Key]
        public int id_territorial { get; set; }
        public int id_departamento { get; set; }
        public int codigo { get; set; }
        public string nombre { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Gnl_Zonas
    {
        [Key]
        public int id_zona { get; set; }
        public int id_territorial { get; set; }
        public int codigo { get; set; }
        public string nombre { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Tipos_Suspencion
    {
        [Key]
        public int id_tipo_suspencion { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int id_actividad { get; set; }
        public Op360ParametrosInicialesAreaCentral_Tipos_brigada[] tipos_brigada { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Tipos_brigada
    {
        [Key]
        public int id_tipo_brigada { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Anomalias
    {
        [Key]
        public int id_anomalia { get; set; }
        public int id_tipo_orden { get; set; }
        public string descripcion { get; set; }
        public Op360ParametrosInicialesAreaCentral_SubAnomalias[] sub_anomalias { get; set; }
    }
    
    public class Op360ParametrosInicialesAreaCentral_SubAnomalias
    {
        [Key]
        public int id_subanomalia { get; set; }        
        public string descripcion { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Estados_Servicio
    {
        [Key]
        public int id_estado_servicio { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }

    public class Op360ParametrosInicialesAreaCentral_Parametros_Generales
    {
        [Key]        
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string valor { get; set; }
    }
}
